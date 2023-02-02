using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MusicPlayer
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage bitmapImage;
            if (value != null)
            {
                bitmapImage = GetBitmapImage((byte[])value);
                return bitmapImage;
            }
            else
            {
                BitmapImage bitmap = new BitmapImage(new Uri(@"/MusicPlayer;component/Image/1.jpg", UriKind.RelativeOrAbsolute));
                return bitmap;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static BitmapImage GetBitmapImage(byte[] imageBytes)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageBytes);
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
