#region Using directives

using System;
using System.Linq;
using System.Threading.Tasks;
using CPECentral.Data.EF5;
using SalesOrderManagerWPF.ViewModels;
using SalesOrderManagerWPF.Views;
using SalesOrderParser;
using Tricorn;

#endregion

namespace SalesOrderManagerWPF.Presenters
{
    public class SalesOrderPresenter
    {
        private readonly SalesOrderView _view;

        public SalesOrderPresenter(SalesOrderView view)
        {
            _view = view;
        }

        public async Task RetrieveViewModelAsync(SalesOrderDetail orderDetail)
        {
            var model = await Task.Factory.StartNew(() =>
            {
                byte[] bytes = null;
                string pathToDrawingFile = null;
                string partName = null;
                string customerName = null;
                string quoteNumber = null;
                string woNumber = null;
                DateTime? date = null;

                using (var cpe = new CPEUnitOfWork())
                {
                    var version = cpe.PartVersions.GetById(46);
                    bytes = cpe.Photos.GetByPartVersion(version);
                    var part = cpe.Parts.GetWhereDrawingNumberMatches(orderDetail.DrawingNumber).SingleOrDefault();
                    if (part != null)
                    {
                        partName = part.Name;
                        customerName = part.Customer.Name;

                        var latestVersion = cpe.PartVersions.GetLatestVersion(part);
                        bytes = cpe.Photos.GetByPartVersion(latestVersion);
                        if (latestVersion.DrawingDocumentId.HasValue)
                        {
                            var doc = cpe.Documents.GetById(latestVersion.DrawingDocumentId.Value);
                            pathToDrawingFile = cpe.Documents.GetPathToDocument(doc, cpe);
                        }
                    }
                }

                using (var tricorn = new TricornDataProvider())
                {
                    quoteNumber = tricorn.GetLastQuoteNumber(orderDetail.DrawingNumber, out date);

                    woNumber = tricorn.GetLastWorksOrderNumber(orderDetail.DrawingNumber);
                }

                var viewModel = new SalesOrderViewModel
                {
                    Customer = customerName,
                    OrderNumber = orderDetail.OrderNumber,
                    DeliveryDate = orderDetail.DeliveryDate,
                    DrawingNumber = orderDetail.DrawingNumber,
                    Name = partName,
                    LastWorksOrderNumber = woNumber,
                    LastQuoteNumber = quoteNumber,
                    LastQuotedOn = date,
                    SalesOrderFileName = orderDetail.FileName,
                    DrawingFileName = pathToDrawingFile,
                    PhotoBytes = bytes
                };

                return viewModel;
            });

            _view.DataContext = model;
        }
    }
}