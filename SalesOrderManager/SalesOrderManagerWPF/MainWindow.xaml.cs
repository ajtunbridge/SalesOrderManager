﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CPECentral.Data.EF5;
using OutlookService;
using SalesOrderManagerWPF.ViewModels;
using SalesOrderParser;
using Tricorn;

namespace SalesOrderManagerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowPresenter _presenter;

        public MainWindow()
        {
            InitializeComponent();

            _presenter = new MainWindowPresenter(this);
        }
        
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await _presenter.RetrieveSalesOrdersFromOutlookAsync();

            OrderView.MarksAsLaunched += OrderView_MarksAsLaunched;
        }

        private void OrderView_MarksAsLaunched(object sender, EventArgs e)
        {
            var salesOrders = OrdersList.ItemsSource as ObservableCollection<SalesOrderDetail>;

            salesOrders.RemoveAt(0);
        }

        public void DisplaySalesOrders(ObservableCollection<SalesOrderDetail> salesOrders)
        {
            Progress.Visibility = Visibility.Collapsed;
            RescanButton.Visibility = Visibility.Visible;

            OrdersListHeader.Text = "Sales orders to launch";

            OrdersList.ItemsSource = salesOrders;
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

            var salesOrder = OrdersList.SelectedItem as SalesOrderDetail;
            
            OrderView.SetSalesOrder(salesOrder);
        }

        private async void RescanButton_Click(object sender, RoutedEventArgs e)
        {
            OrderView.SetSalesOrder(null);
            OrdersList.ItemsSource = null;

            Progress.Visibility = Visibility.Visible;
            RescanButton.Visibility = Visibility.Collapsed;
            OrdersListHeader.Text = "Retrieving sales orders from Outlook";

            await _presenter.RetrieveSalesOrdersFromOutlookAsync();
        }
    }
}
