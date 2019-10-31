using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
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
                RaiseStaticPropertyChanged("CurrentX");
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
                RaiseStaticPropertyChanged("CurrentY");
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
                RaiseStaticPropertyChanged("CurrentZ");
            }
        }

        public DispatcherTimer UpdateTimer { get; set; }

        public DispatcherTimer PollTimer { get; set; }

        public bool Connected { get; set; }

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
                RaiseStaticPropertyChanged("CurrentSelectedPortName");
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
                RaiseStaticPropertyChanged("CurrentSelectedBaudRate");
            }
        }



        public CNC_Device Device { get; set; }

        #region Static INotifyPropertyChanged

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static void RaiseStaticPropertyChanged(string PropName)
        {
            EventHandler<PropertyChangedEventArgs> handler = StaticPropertyChanged;
            if (handler != null)
            {
                handler(null, new PropertyChangedEventArgs(PropName));
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

                    //Delete old device and, most importantly, close old connection
                    if (Device != null)
                    {
                        Device.Interface.CloseConnection();
                    }
                    // Make new device
                    CNCInterface iface = new SerialGRBLInterface(CurrentSelectedPortName, CurrentSelectedBaudRate);
                    CNCProtokoll protokoll = new GRBLProtokoll();
                    Device = new CNC_Device(iface, protokoll);
                    Device.PropertyChanged += Device_PropertyChanged;
                    Device.CurrentX = 5;

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
                    PresentViewModel.CurrentX = Device.CurrentX;
                    break;

                case nameof(Device.CurrentY):
                    PresentViewModel.CurrentY = Device.CurrentY;
                    break;

                case nameof(Device.CurrentZ):
                    PresentViewModel.CurrentZ = Device.CurrentZ;
                    break;

                default:
                    break;
            }
        }

        private void PresentViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Device):

                    break;
                default:
                    break;
            }
        }
    }
}
