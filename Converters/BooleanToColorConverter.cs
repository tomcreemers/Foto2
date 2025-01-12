using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Foto2.Converters
{
    public class BooleanToColorConverter : IValueConverter
    {
        public Color UserColor { get; set; } = Colors.LightBlue;
        public Color AIColor { get; set; } = Colors.LightGray;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isUser)
            {
                return isUser ? UserColor : AIColor;
            }
            return AIColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
