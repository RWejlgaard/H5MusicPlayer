using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MusicPlayer.Properties;

namespace MusicPlayer
{
    public class BoolPlayPauseImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if ((bool)value) {
                return "\\Resources\\Pause-96.png";
            }
            return "\\Resources\\Play-96.png";

            //return (bool)value ? "\\Resources\\Pause-96.png" : "\\Resources\\Play-96.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}