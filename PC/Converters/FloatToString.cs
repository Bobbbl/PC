using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace PC
{
    public class FloatToString : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return String.Format("{0:0.00}", System.Convert.ToDouble(value));

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new FloatToString();
        }
    }
}
