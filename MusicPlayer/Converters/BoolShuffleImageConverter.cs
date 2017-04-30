using System;
using System.Windows.Data;

namespace MusicPlayer.Converters
{
    class BoolShuffleImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (bool)value)
            {
                return "\\Resources\\Shuffle-Enabled-96.png";
            }
            return "\\Resources\\Shuffle-96.png";

            //return (bool)value ? "\\Resources\\Pause-96.png" : "\\Resources\\Play-96.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
