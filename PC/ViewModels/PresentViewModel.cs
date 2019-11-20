using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace PC
{
    public class PresentViewModel : BaseViewModel
    {
        public List<string> ConsoleText { get; set; }

        private static double _CurrentX;
        public static double CurrentX
        {
            get
            {
                return _CurrentX;
            }
            set
            {
                _CurrentX = value;
                RaiseStaticPropertyChanged(nameof(CurrentX));
            }
        }

        private static double _CurrentY;
        public static double CurrentY
        {
            get
            {
                return _CurrentY;
            }
            set
            {
                _CurrentY = value;
                RaiseStaticPropertyChanged(nameof(CurrentY));
            }
        }

        private static double _CurrentZ;
        public static double CurrentZ
        {
            get
            {
                return _CurrentZ;
            }
            set
            {
                _CurrentZ = value;
                RaiseStaticPropertyChanged(nameof(CurrentZ));
            }
        }

        public DispatcherTimer UpdateTimer { get; set; }

        public DispatcherTimer PollTimer { get; set; }

        public bool ErrorReceived { get; set; }


        private static string _CurrentSelectedPortName;
        public static string CurrentSelectedPortName
        {
            get
            {
                return _CurrentSelectedPortName;
            }
            set
            {
                _CurrentSelectedPortName = value;
                RaiseStaticPropertyChanged(nameof(CurrentSelectedPortName));

            }
        }

        private static int _CurrentSelectedBaudRate;

        public static int CurrentSelectedBaudRate
        {
            get
            {
                return _CurrentSelectedBaudRate;
            }
            set
            {
                _CurrentSelectedBaudRate = value;
                RaiseStaticPropertyChanged(nameof(CurrentSelectedBaudRate));
            }
        }



        public static CNC_Device _Device;
        public static CNC_Device Device
        {
            get
            {
                return _Device;
            }
            set
            {
                if (_Device != value)
                {
                    _Device = value;
                    RaiseStaticPropertyChanged(nameof(Device));
                }

            }
        }

        #region Static INotifyPropertyChanged

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static void RaiseStaticPropertyChanged(string PropName)
        {
            EventHandler<PropertyChangedEventArgs> handler = StaticPropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(PropName);
                handler(typeof(PresentViewModel), e);
            }
        }

        #endregion

        public PresentViewModel()
        {
            this.PropertyChanged += PresentViewModel_PropertyChanged;
            StaticPropertyChanged += PresentViewModel_StaticPropertyChanged;
        }

        /// <summary>
        /// This Method gets called every time a static method is changed. Be !careful!, this method only gets called if the
        /// value actually change. If the new value is the same as before, than it not get changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PresentViewModel_StaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(PresentViewModel.CurrentSelectedPortName):

                    if (Device != null && Device.Interface.Portname == CurrentSelectedPortName)
                    {
                        Device.Interface.CloseConnection();
                        Device.RefreshInterval = 0;
                        _Device = null;
                        return;
                    }

                    //Delete old device and, most importantly, close old connection
                    if (Device != null)
                    {
                        Device.Interface.CloseConnection();
                        Device.RefreshInterval = 0;
                        _Device = null;
                    }



                    // Make new device
                    CNCInterface iface = new SerialGRBLInterface(CurrentSelectedPortName, CurrentSelectedBaudRate);
                    CNCProtokoll protokoll = new GRBLProtokoll();

                    _Device = new CNC_Device(iface, protokoll);
                    (iface as SerialGRBLInterface).PortOpened += (s, k) =>
                    {
                        ToolbarViewModel.IsConnected = true;
                    };
                    (iface as SerialGRBLInterface).OpenPortFailed += (s, k) =>
                    {
                        ToolbarViewModel.IsConnected = false;
                    };
                    (iface as SerialGRBLInterface).PortClosed += (s, k) =>
                    {
                        ToolbarViewModel.IsConnected = false;
                        Device.RefreshInterval = 0;
                    };
                    Device.PropertyChanged += Device_PropertyChanged;
                    (iface as SerialGRBLInterface).FirePortOpened();
                    Device.SendReceiveBuffer.CollectionChanged += (s, k) =>
                    {
                        if(k.NewItems == null)
                        {
                            return;
                        }

                        foreach (var item in k.NewItems)
                        {
                            ConsoleViewModel.ConsoleContent += item;
                            ConsoleViewModel.ConsoleContent += "\n";
                        }
                    };

                    // TODO: Add Refresh Rate Textbox to ToolbarPresenter and bind it!
                    Device.RefreshInterval = 100;

                    break;

                case nameof(Device):


                    break;


                default:
                    break;
            }


        }

        /// <summary>
        /// This gets fired if a Device Variable is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Device_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Device.CurrentX):
                    double x = Device.CurrentX;
                    PresentViewModel.CurrentX = x;
                    PlotViewModel.SetWheelXPosition(x);
                    break;

                case nameof(Device.CurrentY):
                    double y = Device.CurrentY;
                    PresentViewModel.CurrentY = y;
                    PlotViewModel.SetWheelYPosition(y);
                    break;

                case nameof(Device.CurrentZ):
                    double z = Device.CurrentZ;
                    PresentViewModel.CurrentZ = z;
                    PlotViewModel.SetWheelZPosition(z);
                    break;

                default:
                    break;
            }
        }

        private void PresentViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                default:
                    break;
            }
        }
    }
}
