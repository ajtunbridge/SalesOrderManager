using System;
using System.Collections.Generic;
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
using SalesOrderManagerWPF.Presenters;
using SalesOrderParser;

namespace SalesOrderManagerWPF.Views
{
    /// <summary>
    /// Interaction logic for SalesOrderView.xaml
    /// </summary>
    public partial class SalesOrderView : UserControl
    {
        public event EventHandler MarkAsLaunched;
        private SalesOrderPresenter _presenter;
        public SalesOrderDetail OrderDetail { get; private set; }


        public SalesOrderView()
        {
            InitializeComponent();

            _presenter = new SalesOrderPresenter(this);
        }

        private void HeaderText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;

            if (textBlock == null)
            {
                return;
            }

            Clipboard.SetText(textBlock.Text);
        }

        public async void SetSalesOrder(SalesOrderDetail salesOrder)
        {
            OrderDetail = salesOrder;

            if (salesOrder == null)
            {
                SalesOrderViewer.Source = null;
                FindDrawingView.ResetView();
            }
            else
            {
                Visibility = Visibility.Hidden;

                await _presenter.RetrieveViewModelAsync(salesOrder);

                FindDrawingView.SearchByDrawingNumber(salesOrder.DrawingNumber);

                Visibility = Visibility.Visible;
            }
        }

        public void OrderMarkedAsLaunched()
        {
            OnMarkAsLaunched();
        }

        protected virtual void OnMarkAsLaunched()
        {
            MarkAsLaunched?.Invoke(this, EventArgs.Empty);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _presenter.MoveEmailToLaunchedFolder();
        }
    }
}
