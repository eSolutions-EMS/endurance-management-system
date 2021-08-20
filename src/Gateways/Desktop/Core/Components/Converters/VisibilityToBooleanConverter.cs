using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EnduranceJudge.Gateways.Desktop.Core.Components.Converters
{
    [ValueConversion(typeof(Visibility), typeof(bool))]
    public class VisibilityToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Visibility visibility)
            {
                throw new InvalidOperationException($"Cannot convert '{value}' to bool.");
            }

            return visibility == Visibility.Visible;;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool boolean)
            {
               throw new InvalidOperationException($"Cannot convert value '{value}' to {nameof(Visibility)} enum.");
            }

            if (boolean)
            {
                return Visibility.Visible;
            }

            return Visibility.Hidden;
        }
    }
}
