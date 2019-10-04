using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace PC
{
    public class PortToPortisConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(List<string>))
            {
                throw new ApplicationException("Converter PortToPortisConverter expected string");
            }

            List<PC.XAMLFiles.Portis> portislist = new List<XAMLFiles.Portis>();
            List<string> PortList = (List<string>)value;

            foreach (string item in PortList)
            {
                portislist.Add(new XAMLFiles.Portis() { PortName = item, BaudRate = 115200 });
            }
            return portislist;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new PortToPortisConverter();
        }
    }
}
