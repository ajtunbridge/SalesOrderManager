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
using SalesOrderParser;

namespace SalesOrderManagerWPF
{
    public class MainWindowPresenter
    {
        private MainWindow _mainWindow;

        public MainWindowPresenter(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public async Task RetrieveSalesOrdersFromOutlookAsync()
        {
            var salesOrders = await GetSalesOrdersAsync();

            var orderedCollection = new ObservableCollection<SalesOrderDetail>();

            foreach (var order in salesOrders.OrderBy(so => so.DeliveryDate))
            {
                orderedCollection.Add(order);
            }

            _mainWindow.DisplaySalesOrders(orderedCollection);
        }

        private async Task<ObservableCollection<SalesOrderDetail>> GetSalesOrdersAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                var salesOrderAttachments = OutlookDataProvider.ExtractSalesOrderPdfs(
                    Settings.Default.NewOrderFolderName,
                    Path.GetTempPath());

                string orderExpr, buyerExpr, deliveryExpr, drawingExpr;

                using (var cpe = new CPEUnitOfWork())
                {
                    var customer = cpe.Customers.GetAll().FirstOrDefault(c => c.Name.Contains("E2V"));
                    orderExpr = customer.OrderNumberRegex;
                    buyerExpr = customer.BuyerRegex;
                    deliveryExpr = customer.DeliveryDateRegex;
                    drawingExpr = customer.DrawingNumberRegex;
                }

                var salesOrders = new ObservableCollection<SalesOrderDetail>();

                foreach (var attachment in salesOrderAttachments)
                {
                    var detail = PdfParser.ParseSalesOrderAsync(attachment.FileName, orderExpr,
                        deliveryExpr, buyerExpr, drawingExpr).Result;

                    salesOrders.Add(detail);
                }

                return salesOrders;
            });
        }
    }
}