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

        public static double CurrentX { get; set; }

        public static double CurrentY { get; set; }

        public static double CurrentZ { get; set; }

        public DispatcherTimer UpdateTimer { get; set; }

        public DispatcherTimer PollTimer { get; set; }


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


        public static CNC_Device Device { get; set; }

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
