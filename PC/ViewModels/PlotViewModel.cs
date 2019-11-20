using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace PC
{
    class PlotViewModel : BaseViewModel
    {
        #region Static INotifyPropertyChanged

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static void RaiseStaticPropertyChanged(string PropName)
        {
            EventHandler<PropertyChangedEventArgs> handler = StaticPropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(PropName);
                handler(typeof(PlotViewModel), e);
            }
        }

        #endregion

        public static Point3D _WheelPosition = new Point3D(0, 0, 0);
        public static Point3D WheelPosition
        {
            get
            {
                return _WheelPosition;
            }
            set
            {
                if(_WheelPosition.X != value.X &&
                    _WheelPosition.Y != value.Y &&
                    _WheelPosition.Z != value.Z)
                {
                    _WheelPosition = value;
                    RaiseStaticPropertyChanged(nameof(WheelPosition));
                }
            }
        }
        public static void SetWheelXPosition(double x)
        {
            _WheelPosition.X = x;
            RaiseStaticPropertyChanged(nameof(WheelPosition));
        }
        public static void SetWheelYPosition(double y)
        {
            _WheelPosition.Y = y;
            RaiseStaticPropertyChanged(nameof(WheelPosition));
        }
        public static void SetWheelZPosition(double z)
        {
            _WheelPosition.Z = z;
            RaiseStaticPropertyChanged(nameof(WheelPosition));
        }

    }
}
