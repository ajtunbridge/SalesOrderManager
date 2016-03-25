using System;
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

namespace SalesOrderManagerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GenerateFakeData()
        {
            var salesOrderAttachments = OutlookDataProvider.ExtractSalesOrderPdfs("Sales orders - unlaunched",
                "C:\\Users\\Adam\\Desktop\\Extracted\\");

            string orderExpr, buyerExpr, deliveryExpr, drawingExpr;

            using (var cpe = new CPEUnitOfWork())
            {
                var customer = cpe.Customers.GetAll().FirstOrDefault(c => c.Name.Contains("E2V"));
                
            }

                foreach (var attachment in salesOrderAttachments)
                {
                }

            var result = await Task.Factory.StartNew(() =>
            {
                var order1 = new SalesOrderDetail
                {
                    Buyer = "Duncan Pegrum",
                    DeliveryDate = DateTime.Today.AddDays(-6),
                    OrderNumber = "4500234325",
                    DrawingNumber = "H522922D",
                    FileName = "ERROR!"
                };

                var order2 = new SalesOrderDetail
                {
                    Buyer = "Duncan Pegrum",
                    DeliveryDate = DateTime.Today.AddDays(3),
                    OrderNumber = "4500252225",
                    DrawingNumber = "JPP546192AA",
                    FileName = "ERROR!"
                };


                var order3 = new SalesOrderDetail
                {
                    Buyer = "Peter Smith",
                    DeliveryDate = DateTime.Today.AddDays(43),
                    OrderNumber = "4500212390",
                    DrawingNumber = "R51133B",
                    FileName = "ERROR!"
                };

                var order4 = new SalesOrderDetail
                {
                    Buyer = "Anne Murray",
                    DeliveryDate = DateTime.Today.AddMonths(2),
                    OrderNumber = "4500235127",
                    DrawingNumber = "H17070A",
                    FileName = "ERROR!"
                };

                var order5 = new SalesOrderDetail
                {
                    Buyer = "Duncan Pegrum",
                    DeliveryDate = DateTime.Today.AddDays(2),
                    OrderNumber = "4500244121",
                    DrawingNumber = "JAS756545AA",
                    FileName = "ERROR!"
                };

                return new ObservableCollection<SalesOrderDetail> { order1, order2, order3, order4, order5 };
            });

            OrdersList.ItemsSource = result;

            byte[] bytes = null;

            using (var cpe = new CPEUnitOfWork())
            {
                var version = cpe.PartVersions.GetById(46);
                bytes = cpe.Photos.GetByPartVersion(version);
            }

            var model = new SalesOrderViewModel
            {
                Customer = "E2V TECHN. (UK) LTD  CH",
                OrderNumber = "4500543873",
                DeliveryDate = DateTime.Today.AddDays(15),
                DrawingNumber = "H17070A",
                Name = "Inner Tube",
                LastWorksOrderNumber="14587",
                LastQuoteNumber="22341",
                LastQuotedOn = DateTime.Today.AddDays(-231),
                SalesOrderFileName = @"C:\e2v_Po_4500255191.pdf",
                PhotoBytes = bytes
            };


            OrderView.DataContext = model;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateFakeData();
            OrderView.MarksAsLaunched += OrderView_MarksAsLaunched;
        }

        private void OrderView_MarksAsLaunched(object sender, EventArgs e)
        {
            var salesOrders = OrdersList.ItemsSource as ObservableCollection<SalesOrderDetail>;

            salesOrders.RemoveAt(0);
        }

        private void OrderView_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
