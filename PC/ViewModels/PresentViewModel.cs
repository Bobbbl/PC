using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public string SelectedPortName { get; set; }

        public int SelectedBaudRate { get; set; }


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

    }
}
