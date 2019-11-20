using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Media3D;

namespace PC
{
    public class Point3DToString : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Point3D p = PlotViewModel.WheelPosition;
            string r = string.Empty;

            r += p.X.ToString() + " ";
            r += p.Y.ToString() + " ";
            r += p.Z.ToString();

            return r;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new Point3DToString();
        }
    }
}
