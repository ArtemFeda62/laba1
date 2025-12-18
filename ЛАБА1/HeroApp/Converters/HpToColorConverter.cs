using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HeroApp.WPF.Converters
{
    public class HpToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double hp)
            {
                if (hp <= 0) return Brushes.Gray;
                if (hp < 20) return Brushes.Red;
                if (hp < 50) return Brushes.Orange;
                if (hp < 100) return Brushes.Green;
                return Brushes.DarkGreen;
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}