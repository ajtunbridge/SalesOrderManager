using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SalesOrderManagerWPF.Views
{
    /// <summary>
    /// Interaction logic for ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : UserControl
    {
        private string _fileName;

        public ImageViewer()
        {
            InitializeComponent();
        }

        public void SetImage(string fileName)
        {
            _fileName = fileName;

            if (fileName == null)
            {
                TheImage.Source = null;
            }
            else
            {
                TheImage.Source = new BitmapImage(new Uri(fileName));
            }
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(_fileName);
        }

        private void UIElement_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
        }
    }
}