using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HeroApp.WPF.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && parameter is string paramString)
            {
                var colors = paramString.Split('|');
                if (colors.Length == 2)
                {
                    return boolValue ?
                        new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[0])) :
                        new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[1]));
                }
            }
            return Brushes.LightGreen;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}