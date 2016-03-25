using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SalesOrderManagerWPF.Converters
{
    /// <summary>
    /// Returns true if the date provided is within a month of, or earlier than, today's date
    /// </summary>
    [ValueConversion(typeof(DateTime), typeof(bool))]
    public class DeliveryDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var deliveryDate = (DateTime)value;
            
            if (deliveryDate < DateTime.Today)
            {
                return true;
            }

            var span = deliveryDate.Subtract(DateTime.Today);

            return span.Days < 30;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
