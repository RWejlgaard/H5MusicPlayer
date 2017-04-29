using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MusicPlayer.Converters {
    class TimeSliderDoubleToTimeString : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null) {
                TimeSpan t = TimeSpan.FromSeconds((double) value);

                return $"{t.Minutes}:{t.Seconds:D2}";
            }

            return "0:00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}