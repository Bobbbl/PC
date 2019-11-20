using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Windows.Threading;

namespace PC
{
    public class PortViewModel : BaseViewModel
    {
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


        public List<string> PortList { get; set; } = new List<string>();

        private DispatcherTimer PortSearchTimer = new DispatcherTimer();

        private static List<PC.XAMLFiles.Portis> _PortisList = new List<XAMLFiles.Portis>();
        public static List<PC.XAMLFiles.Portis> PortisList
        {
            get
            {
                return _PortisList;
            }
            set
            {
                if (_PortisList != value)
                {
                    _PortisList = value;
                    RaiseStaticPropertyChanged(nameof(PortisList));
                }

            }
        }


        private void SearchForPortAndAddOrRemove(object sender, EventArgs e)
        {
            List<string> portlist = SerialPort.GetPortNames().ToList();

            var oldports = PortList.Except(portlist).ToList();
            var newports = portlist.Except(PortList).ToList();

            if (oldports.Count == 0 && newports.Count == 0)
            {
                return;
            }

            PortList = portlist;
            OnPropertyChanged(nameof(PortList));

        }


        public PortViewModel()
        {

            PortSearchTimer.Tick += (sender, e) => { SearchForPortAndAddOrRemove(sender, e); };
            this.PropertyChanged += PortViewModel_PropertyChanged;
            PortSearchTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            PortSearchTimer.Start();
        }

        private void PortViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(PortList):

                    List<PC.XAMLFiles.Portis> portislist = new List<XAMLFiles.Portis>();

                    foreach (string item in PortList)
                    {
                        portislist.Add(new XAMLFiles.Portis() { PortName = item, BaudRate = 115200 });
                    }

                    PortisList = portislist;

                    break;
                default:
                    break;
            }
        }
    }
}
