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
        public event EventHandler MarksAsLaunched;
        private SalesOrderPresenter _presenter;

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
            if (salesOrder == null)
            {
                DrawingFileViewer.Source = null;
                SalesOrderViewer.Source = null;
            }
            else
            {
                await _presenter.RetrieveViewModelAsync(salesOrder);
            }
        }

        protected virtual void OnMarksAsLaunched()
        {
            MarksAsLaunched?.Invoke(this, EventArgs.Empty);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnMarksAsLaunched();
        }
    }
}
