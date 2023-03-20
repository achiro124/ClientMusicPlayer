using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MusicPlayer
{
    public class ImgFavoriteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage bitmapImage;
            if ((bool)value)
            {
                bitmapImage = new BitmapImage(new Uri(@"/MusicPlayer;component/Image/Red_Heart.png", UriKind.RelativeOrAbsolute));
                return bitmapImage;
            }
            else
            {
                bitmapImage = new BitmapImage(new Uri(@"/MusicPlayer;component/Image/conturHeart1.png", UriKind.RelativeOrAbsolute));
                return bitmapImage;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
