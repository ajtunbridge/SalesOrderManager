using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SalesOrderManagerWPF.Converters
{
    [ValueConversion(typeof (byte[]), typeof (BitmapImage))]
    public class BinaryImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bytes = value as byte[];

            var bmpImg = new BitmapImage();

            if (bytes == null)
            {
                bmpImg = new BitmapImage(new Uri("/Images/PhotoUnavailableImage.png", UriKind.Relative));
            }
            else
            {
                using (var ms = new MemoryStream(bytes))
                {
                    ms.Position = 0;
                    bmpImg.BeginInit();
                    bmpImg.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bmpImg.CacheOption = BitmapCacheOption.OnLoad;
                    bmpImg.UriSource = null;
                    bmpImg.StreamSource = ms;
                    bmpImg.EndInit();
                }
            }

            return bmpImg;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}