using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace PC
{
    public class ObservableCollectionToString : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(ObservableCollection<string>))
            {
                throw new ApplicationException("Converter PortToPortisConverter expected string");
            }

            var inlist = value as ObservableCollection<string>;

            string rstring = string.Empty;

            foreach (string item in inlist)
            {
                rstring += item;
            }

            return rstring;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new ObservableCollectionToString();
        }
    }
}
