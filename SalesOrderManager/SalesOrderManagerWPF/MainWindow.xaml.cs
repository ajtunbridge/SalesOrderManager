using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using SalesOrderManagerWPF.ViewModels;
using SalesOrderParser;

namespace SalesOrderManagerWPF
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowPresenter _presenter;

        public MainWindow()
        {
            InitializeComponent();

            Uri iconUri = new Uri("pack://application:,,,/Resources/ApplicationIcon.ico", UriKind.RelativeOrAbsolute);

            Icon = BitmapFrame.Create(iconUri);

            _presenter = new MainWindowPresenter(this);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await _presenter.RetrieveSalesOrdersFromOutlookAsync();

            OrderView.MarkAsLaunched += OrderViewMarkAsLaunched;
        }

        private void OrderViewMarkAsLaunched(object sender, EventArgs e)
        {
            RescanButton_Click(sender, null);
        }

        public void DisplaySalesOrders(ObservableCollection<SalesOrderListItemViewModel> salesOrders)
        {
            Progress.Visibility = Visibility.Collapsed;
            RescanButton.Visibility = Visibility.Visible;

            OrdersListHeader.Text = "Sales orders to launch";

            OrdersList.ItemsSource = salesOrders;

            if (OrdersList.Items.Count > 0)
            {
                OrdersList.SelectedIndex = 0;
            }
        }

        private void OrderView_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void OrdersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrdersList.SelectedItem == null)
            {
                return;
            }

            var salesOrder = OrdersList.SelectedItem as SalesOrderListItemViewModel;

            LoadingView.Visibility = Visibility.Visible;

            OrderView.SetSalesOrder(salesOrder);
        }

        private async void RescanButton_Click(object sender, RoutedEventArgs e)
        {
            OrderView.SetSalesOrder(null);
            OrdersList.ItemsSource = null;

            OrderView.Visibility = Visibility.Hidden;
            LoadingView.Visibility = Visibility.Hidden;

            Progress.Visibility = Visibility.Visible;
            RescanButton.Visibility = Visibility.Collapsed;
            OrdersListHeader.Text = "Retrieving sales orders from Outlook";

            await _presenter.RetrieveSalesOrdersFromOutlookAsync();
        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ValueFilterOrderNumberValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            _presenter.FilterWhereOrderNumberContains(ValueFilterOrderNumberValue.Text);
        }
    }
}