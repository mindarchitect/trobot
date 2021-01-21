using System.Windows;
using System.Windows.Data;

namespace TRobot.Core.UI.Converters
{
    public class MarginConverter : IValueConverter
    {
        private const int ValidationMessageBlockHeight = 50; 

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new Thickness(5, 5, 5, System.Convert.ToDouble(value) * ValidationMessageBlockHeight);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
