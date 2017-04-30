using System;
using System.Globalization;
using System.Windows.Data;

namespace MusicPlayer.Converters {
    class VolumeSliderDoubleImageConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null) return "\\Resources\\Mute-96.png";
            var volumeValue = System.Convert.ToDouble(value);

            if (volumeValue > 66.66) {
                return "\\Resources\\HighVolume-96.png";
            }

            if (volumeValue > 33.33) {
                return "\\Resources\\MediumVolume-96.png";
            }

            if (volumeValue > 0.0) {
                return "\\Resources\\LowVolume-96.png";
            }

            return "\\Resources\\Mute-96.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}