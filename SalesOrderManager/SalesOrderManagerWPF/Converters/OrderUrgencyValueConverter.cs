using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SalesOrderManagerWPF.Converters
{
    [ValueConversion(typeof(DateTime), typeof(SolidColorBrush))]
    public sealed class OrderUrgencyValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var deliveryDate = (DateTime) value;

            var span = deliveryDate.Subtract(DateTime.Today);

            if (span.Days <= 7)
            {
                return new SolidColorBrush(Colors.Red);
            }

            if (span.Days <= 31)
            {
                return new SolidColorBrush(Colors.Orange);
            }

            return new SolidColorBrush(Colors.LimeGreen);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
