using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PC
{
    public class ToolbarViewModel : BaseViewModel
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

        public int StepSizeJog { get; set; }

        public int FeedRateJog { get; set; }

        public int SpindelSpeed { get; set; }

        public bool SpindelIsOn { get; set; }

        private static bool _IsConnected;

        public static bool IsConnected
        {
            get
            {
                return _IsConnected;
            }
            set
            {
                _IsConnected = value;
                RaiseStaticPropertyChanged("IsConnected");
            }
        }



        public ICommand SetZeroCommand { get; set; }
        public ICommand HomingCommand { get; set; }
        public ICommand UnlockCommand { get; set; }
        public ICommand XMinusCommand { get; set; }
        public ICommand XPlusCommand { get; set; }
        public ICommand YPlusCommand { get; set; }
        public ICommand YMinusCommand { get; set; }
        public ICommand ZPlusCommand { get; set; }
        public ICommand ZMinusCommand { get; set; }
        public ICommand SpindelCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand SendLineButtonCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand SendCommand { get; set; }


    }
}
