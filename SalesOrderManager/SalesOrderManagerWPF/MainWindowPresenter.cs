using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPECentral.Data.EF5;
using OutlookService;
using SalesOrderManagerWPF.Properties;
using SalesOrderManagerWPF.ViewModels;
using SalesOrderParser;
using Tricorn;

namespace SalesOrderManagerWPF
{
    public class MainWindowPresenter
    {
        private MainWindow _mainWindow;
        private List<SalesOrderListItemViewModel> _allSalesOrders;
        private ObservableCollection<SalesOrderListItemViewModel> _filteredSalesOrders;
         
        public MainWindowPresenter(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void FilterWhereOrderNumberContains(string value)
        {
            _filteredSalesOrders.Clear();

            var filtered = _allSalesOrders.Where(so => so.OrderNumber.Contains(value));

            foreach (var so in filtered.OrderBy(so => so.DeliveryDate))
            {
                _filteredSalesOrders.Add(so);
            }
        }

        public async Task RetrieveSalesOrdersFromOutlookAsync()
        {
            _allSalesOrders = await GetSalesOrdersAsync();

            _filteredSalesOrders = new ObservableCollection<SalesOrderListItemViewModel>();

            foreach (var order in _allSalesOrders.OrderBy(so => so.DeliveryDate))
            {
                _filteredSalesOrders.Add(order);
            }

            _mainWindow.DisplaySalesOrders(_filteredSalesOrders);
        }

        private async Task<List<SalesOrderListItemViewModel>> GetSalesOrdersAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                var salesOrderAttachments = OutlookDataProvider.ExtractSalesOrderPdfs(
                    Settings.Default.NewOrderFolderName,
                    Path.GetTempPath());

                string orderExpr, buyerExpr, deliveryExpr, drawingExpr;
                byte[] logoBytes = null;

                using (var cpe = new CPEUnitOfWork())
                {
                    var customer = cpe.Customers.GetAll().FirstOrDefault(c => c.Name.Contains("E2V"));
                    orderExpr = customer.OrderNumberRegex;
                    buyerExpr = customer.BuyerRegex;
                    deliveryExpr = customer.DeliveryDateRegex;
                    drawingExpr = customer.DrawingNumberRegex;
                    logoBytes = customer.LogoBLOB;
                }

                var salesOrders = new List<SalesOrderListItemViewModel>();

                foreach (var attachment in salesOrderAttachments)
                {
                    var detail = PdfParser.ParseSalesOrderAsync(attachment.FileName, attachment.MailId, orderExpr,
                        deliveryExpr, buyerExpr, drawingExpr).Result;

                    var model = new SalesOrderListItemViewModel
                    {
                        Buyer=detail.Buyer, DeliveryDate=detail.DeliveryDate, DrawingNumber=detail.DrawingNumber, OrderNumber = detail.OrderNumber,
                        CompanyLogoBytes=logoBytes, FileName=detail.FileName, MailId=detail.MailId
                    };

                    salesOrders.Add(model);
                }

                return salesOrders;
            });
        }
    }
}