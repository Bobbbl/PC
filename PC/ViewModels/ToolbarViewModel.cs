using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

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

        public string CustomLineContent { get; set; }

        public string CNCFileContent { get; set; }

        private static bool _IsConnected;
        public static bool IsConnected
        {
            get
            {
                return _IsConnected;
            }
            set
            {
                if (value != _IsConnected)
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
            CNCMessage m = new CNCMessage() { Message = CustomLineContent };
            await PresentViewModel.Device.SendCustomMessage(m);
        }

        public async Task Load()
        {
            OpenFileDialog odialog = new OpenFileDialog();
            if (odialog.ShowDialog() == true)
            {
                using (StreamReader sr = File.OpenText(odialog.FileName))
                {
                    CNCFileContent = sr.ReadToEnd();
                }
            }
        }

        public async Task Send()
        {
            throw new NotImplementedException();
        }

        #endregion

        public ToolbarViewModel()
        {
            SetZeroCommand = new RelayCommand(async () => await SetZero());
            (SetZeroCommand as RelayCommand).CANPointer += () => { return false; };
            HomingCommand = new RelayCommand(async () => await Homing());
            (HomingCommand as RelayCommand).CANPointer += () => { return false; };
            UnlockCommand = new RelayCommand(async () => await Unlock());
            (UnlockCommand as RelayCommand).CANPointer += () => { return false; };
            XMinusCommand = new RelayCommand(async () => await XMinus());
            (XMinusCommand as RelayCommand).CANPointer += () => { return false; };
            XPlusCommand = new RelayCommand(async () => await XPlus());
            (XPlusCommand as RelayCommand).CANPointer += () => { return false; };
            YPlusCommand = new RelayCommand(async () => await YPlus());
            (YPlusCommand as RelayCommand).CANPointer += () => { return false; };
            YMinusCommand = new RelayCommand(async () => await YMinus());
            (YMinusCommand as RelayCommand).CANPointer += () => { return false; };
            ZPlusCommand = new RelayCommand(async () => await ZPlus());
            (ZPlusCommand as RelayCommand).CANPointer += () => { return false; };
            ZMinusCommand = new RelayCommand(async () => await ZMinus());
            (ZMinusCommand as RelayCommand).CANPointer += () => { return false; };
            SpindelCommand = new RelayCommand(async () => await Spindel());
            (SpindelCommand as RelayCommand).CANPointer += () => { return false; };
            ResetCommand = new RelayCommand(async () => await Reset());
            (ResetCommand as RelayCommand).CANPointer += () => { return false; };
            SendLineButtonCommand = new RelayCommand(async () => await SendLine());
            (SendLineButtonCommand as RelayCommand).CANPointer += () => { return false; };
            LoadCommand = new RelayCommand(async () => await Load());

            SendCommand = new RelayCommand(async () => await Send());
            (SendCommand as RelayCommand).CANPointer += delegate { return IsConnected; };

            StaticPropertyChanged += ToolbarViewModel_StaticPropertyChanged;
        }

        private void ToolbarViewModel_StaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IsConnected):
                    if (IsConnected)
                    {
                        PortViewModel.PortisList.ForEach(porti => porti.IndicatorColor = Brushes.Transparent);

                        XAMLFiles.Portis p = PortViewModel.PortisList.Find(
                            porti =>
                            (PresentViewModel.Device.Interface as SerialGRBLInterface).SerialInterface.PortName == porti.PortName);
                        p.IndicatorColor = Brushes.ForestGreen;
                    }
                    else
                    {
                        PortViewModel.PortisList.ForEach(porti => porti.IndicatorColor = Brushes.Transparent);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
