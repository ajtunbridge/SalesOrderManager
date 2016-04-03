#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using SalesOrderManagerWPF.Presenters;
using SalesOrderManagerWPF.ViewModels;

#endregion

namespace SalesOrderManagerWPF.Views
{
    /// <summary>
    ///     Interaction logic for FindDrawingView.xaml
    /// </summary>
    public partial class FindDrawingView : UserControl
    {
        private readonly List<Tuple<string, BitmapSource>> _fileIcons = new List<Tuple<string, BitmapSource>>();
        private readonly FindDrawingPresenter _presenter;

        private List<FindDrawingViewModel> _searchResults;

        private string _selectedFileName;

        public FindDrawingView()
        {
            InitializeComponent();

            _presenter = new FindDrawingPresenter(this);

            ResultsList.ItemsSource = _searchResults;
        }

        public void SearchByDrawingNumber(string drawingNumber)
        {
            ResultsList.IsEnabled = false;

            _searchResults = new List<FindDrawingViewModel>();
            ResultsList.ItemsSource = null;

            ResultsHeader.Text = $"{_searchResults.Count} results found.";

            Progress.Visibility = Visibility.Visible;

            _presenter.SearchForFiles(drawingNumber);
        }

        public void AddResult(FindDrawingViewModel model)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                model.Icon = GetFileIcon(model.FileName);

                _searchResults.Add(model);

                ResultsHeader.Text = $"{_searchResults.Count} results found.";            
            });
        }

        public void SearchComplete()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Progress.Visibility = Visibility.Collapsed;

                ResultsList.IsEnabled = true;
                
                ResultsList.ItemsSource = _searchResults.OrderByDescending(r => r.CreatedAt);

                if (ResultsList.Items.Count > 0 && ResultsList.SelectedItem == null)
                {
                    ResultsList.SelectedIndex = 0;
                }
            });
        }

        public void ResetView()
        {
            PdfViewer.Source = null;
            ImageViewer.SetImage(null);
            ResultsList.ItemsSource = null;
        }

        private void ResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResultsList.SelectedItem == null)
            {
                PdfViewer.Source = null;
                ImageViewer.SetImage(null);
                return;
            }

            var model = ResultsList.SelectedItem as FindDrawingViewModel;

            _selectedFileName = model.FileName;

            var extension = Path.GetExtension(model.FileName).ToLower();

            var imageExtensions = new List<string> {".tiff", ".tif", ".bmp", ".jpeg", "*.jpg"};

            if (extension == ".pdf")
            {
                PdfViewer.Source = new Uri(model.FileName);
                PdfViewer.Visibility = Visibility.Visible;

                ImageViewer.Visibility = Visibility.Hidden;
                ViewExternallyButton.Visibility = Visibility.Hidden;
            }
            else if (imageExtensions.Any(ext => ext == extension))
            {
                ImageViewer.SetImage(model.FileName);
                ImageViewer.Visibility = Visibility.Visible;

                PdfViewer.Visibility = Visibility.Hidden;
                ViewExternallyButton.Visibility = Visibility.Hidden;
            }
            else
            {
                PdfViewer.Visibility = Visibility.Hidden;
                ImageViewer.Visibility = Visibility.Hidden;
                ViewExternallyButton.Visibility = Visibility.Visible;
            }
        }

        private BitmapSource GetFileIcon(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            var existing = _fileIcons.FirstOrDefault(t => t.Item1 == extension);

            if (existing != null)
            {
                return existing.Item2;
            }

            var sysicon = Icon.ExtractAssociatedIcon(fileName);

            var bmpSrc = Imaging.CreateBitmapSourceFromHIcon(
                sysicon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            sysicon.Dispose();

            _fileIcons.Add(new Tuple<string, BitmapSource>(extension, bmpSrc));

            return bmpSrc;
        }

        private void ViewExternallyButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(_selectedFileName);
            }
            catch
            {
                MessageBox.Show("An error occurred while trying to open this file!");
            }
        }
    }
}