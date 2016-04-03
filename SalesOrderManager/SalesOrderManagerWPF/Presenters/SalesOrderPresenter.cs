#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CPECentral.Data.EF5;
using OutlookService;
using SalesOrderManagerWPF.Properties;
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

        public void MoveEmailToLaunchedFolder()
        {
            if (MessageBox.Show("This will move this sales order to the launched folder!\n\nDo you wish to continue?",
                "Confirm complete",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            var ok = OutlookDataProvider.MoveMail(_view.OrderDetail.MailId, Settings.Default.LaunchedFolderName);

            if (!ok)
            {
                MessageBox.Show(
                    "Unable to move email to the launched folder.\n\nYou can try again or move it manually within Outlook.");

                _view.OrderMarkedAsLaunched();
            }
            else
            {
                _view.OrderMarkedAsLaunched();
            }
        }

        public async Task RetrieveViewModelAsync(SalesOrderDetail orderDetail)
        {
            var model = await Task.Factory.StartNew(() =>
            {
                byte[] bytes = null;
                string pathToDrawingFile = null;
                string partName = null;
                string customerName = null;
                string quoteReference = null;
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
                    }
                }

                List<QuoteDetail> details = new List<QuoteDetail>();

                using (var tricorn = new TricornDataProvider())
                {
                    quoteReference = tricorn.GetLastQuoteGroupReference(orderDetail.DrawingNumber, out date);

                    woNumber = tricorn.GetLastWorksOrderNumber(orderDetail.DrawingNumber);

                    if (quoteReference != null)
                    {
                        details = tricorn.GetQuoteDetails(quoteReference, orderDetail.DrawingNumber);
                    }
                }

                var viewModel = new SalesOrderViewModel
                {
                    Customer = customerName,
                    OrderNumber = orderDetail.OrderNumber,
                    DeliveryDate = orderDetail.DeliveryDate,
                    DrawingNumber = orderDetail.DrawingNumber,
                    Name = partName,
                    LastWorksOrderNumber = woNumber,
                    LastGroupReference = quoteReference,
                    LastQuotedOn = date,
                    SalesOrderFileName = orderDetail.FileName,
                    PhotoBytes = bytes,
                    LastQuoteDetails = details
                };

                return viewModel;
            });

            _view.DataContext = model;
        }
    }
}