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

namespace SalesOrderManagerWPF.Views
{
    /// <summary>
    /// Interaction logic for SalesOrderView.xaml
    /// </summary>
    public partial class SalesOrderView : UserControl
    {
        public event EventHandler MarksAsLaunched;

        public SalesOrderView()
        {
            InitializeComponent();
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
