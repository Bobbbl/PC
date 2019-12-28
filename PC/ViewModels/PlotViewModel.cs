using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
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

        private static List<Point3D> _LineList = new List<Point3D>();

        public static List<Point3D> LineList
        {
            get
            {
                return _LineList;
            }
            set
            {
                _LineList = value;
                RaiseStaticPropertyChanged(nameof(LineList));
            }
        }

        private static List<Point3D> _PointList = new List<Point3D>();

        public static List<Point3D> PointList
        {
            get
            {
                return _PointList;
            }
            set
            {
                _PointList = value;
                RaiseStaticPropertyChanged(nameof(PointList));
            }
        }



        public static Point3DCollection ArcList { get; set; } = new Point3DCollection();

        public static Point3DCollection TubeList { get; set; } = new Point3DCollection();


        public static HelixToolkit.Wpf.HelixViewport3D CurrentViewport;


        public static HelixToolkit.Wpf.HelixViewport3D GetCurrentViewport(DependencyObject obj)
        {
            return (HelixToolkit.Wpf.HelixViewport3D)obj.GetValue(CurrentViewportProperty);
        }

        public static void SetCurrentViewport(DependencyObject obj, HelixToolkit.Wpf.HelixViewport3D value)
        {
            obj.SetValue(CurrentViewportProperty, value);
        }

        // Using a DependencyProperty as the backing store for CurrentViewport.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentViewportProperty =
            DependencyProperty.RegisterAttached("CurrentViewport", typeof(object), typeof(PlotViewModel), new PropertyMetadata(default(object), CurrentViewportChanged));



        private static void CurrentViewportChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CurrentViewport = e.NewValue as HelixToolkit.Wpf.HelixViewport3D;
        }

        private static int _WheelHeight = 2;
        public static int WheelHeight
        {
            get
            {
                return _WheelHeight;
            }
            set
            {

                if (value != _WheelHeight)
                {
                    _WheelHeight = value;
                    RaiseStaticPropertyChanged(nameof(WheelHeight));
                }
            }
        }


        private static int _WheelWidth = 2;
        public static int WheelWidth
        {
            get
            {
                return _WheelWidth;
            }
            set
            {
                if (value != _WheelWidth)
                {
                    _WheelWidth = value;
                    RaiseStaticPropertyChanged(nameof(WheelWidth));
                }

            }
        }

        public static Point3D _WheelPosition = new Point3D(0, 0, 0);
        public static Point3D WheelPosition
        {
            get
            {
                return _WheelPosition;
            }
            set
            {
                if (_WheelPosition.X != value.X &&
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

        #region Constructor

        public PlotViewModel()
        {
            TubeList.Add(new Point3D(0, 0, 0));
            TubeList.Add(new Point3D(2.5548, 22.4311, 0));
            TubeList.Add(new Point3D(0.5567, 26.0488, 0));
        }

        #endregion

    }
}
