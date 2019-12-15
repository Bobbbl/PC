using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Media3D;

namespace PC
{
    public class ListToPoint3DCollection : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            Point3DCollection rcollection = new Point3DCollection();
            List<Point3D> list = (List<Point3D>)value;

            foreach (Point3D item in list)
            {
                rcollection.Add(item);
            }

            return rcollection;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new ListToPoint3DCollection();
        }
    }
}
