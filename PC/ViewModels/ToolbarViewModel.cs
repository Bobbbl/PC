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


        #region Commands
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

        public async Task SetZero()
        {
            await PresentViewModel.Device.SendSetZero();
        }

        public async Task Homing()
        {
            await PresentViewModel.Device.DoHoming();
        }

        public async Task Unlock()
        {
            await PresentViewModel.Device.KillAlarm();
        }

        public async Task XMinus()
        {
            await PresentViewModel.Device.JogX(-StepSizeJog, FeedRateJog);
        }

        public async Task XPlus()
        {
            await PresentViewModel.Device.JogX(StepSizeJog, FeedRateJog);
        }

        public async Task YPlus()
        {
            await PresentViewModel.Device.JogY(StepSizeJog, FeedRateJog);
        }

        public async Task YMinus()
        {
            await PresentViewModel.Device.JogY(-StepSizeJog, FeedRateJog);
        }

        public async Task ZPlus()
        {
            await PresentViewModel.Device.JogZ(StepSizeJog, FeedRateJog);
        }

        public async Task ZMinus()
        {
            await PresentViewModel.Device.JogZ(-StepSizeJog, FeedRateJog);
        }

        public async Task Spindel()
        {
            await PresentViewModel.Device.SetSpindleSpeed(SpindelSpeed, "clockwise");
        }

        public async Task Reset()
        {
            await PresentViewModel.Device.SendSoftReset();
        }

        public async Task SendLine()
        {
            throw new NotImplementedException();
        }

        public async Task Load()
        {
            throw new NotImplementedException();
        }

        public async Task Send()
        {
            throw new NotImplementedException();
        }

        #endregion

        public ToolbarViewModel()
        {
            SetZeroCommand = new RelayCommand(async () => await SetZero());
            HomingCommand = new RelayCommand(async () => await Homing());
            UnlockCommand = new RelayCommand(async () => await Unlock());
            XMinusCommand = new RelayCommand(async () => await XMinus());
            XPlusCommand = new RelayCommand(async () => await XPlus());
            YPlusCommand = new RelayCommand(async () => await YPlus());
            YMinusCommand = new RelayCommand(async () => await YMinus());
            ZPlusCommand = new RelayCommand(async () => await ZPlus());
            ZMinusCommand = new RelayCommand(async () => await ZMinus());
            SpindelCommand = new RelayCommand(async () => await Spindel());
            ResetCommand = new RelayCommand(async () => await Reset());
            SendLineButtonCommand = new RelayCommand(async () => await SendLine());
            LoadCommand = new RelayCommand(async () => await Load());
            SendCommand = new RelayCommand(async () => await Send());
        }

    }
}
