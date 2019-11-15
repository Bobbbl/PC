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
                handler(typeof(ToolbarViewModel), new PropertyChangedEventArgs(PropName));
            }
        }
        #endregion

        public int StepSizeJog { get; set; }

        public int FeedRateJog { get; set; }

        public int SpindelSpeed { get; set; }

        public bool SpindelIsOn { get; set; }

        public string CustomLineContent { get; set; }

        public string CNCFileContent { get; set; }

        private static bool _IsConnected = false;
        public static bool IsConnected
        {
            get
            {
                return _IsConnected;
            }
            set
            {
                if (value != _IsConnected)
                {
                    _IsConnected = value;
                    RaiseStaticPropertyChanged("IsConnected");
                }

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
            var task = PresentViewModel.Device.SendSetZero();
            task.RunSynchronously();
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
            var task = PresentViewModel.Device.JogX(-StepSizeJog, FeedRateJog);
            task.RunSynchronously();

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
            SetZeroCommand = new RelayCommand( () =>  SetZero());
            (SetZeroCommand as RelayCommand).CANPointer += () => { return IsConnected; };
            HomingCommand = new RelayCommand(async () => await Homing());
            (HomingCommand as RelayCommand).CANPointer += () => { return IsConnected; };
            UnlockCommand = new RelayCommand(async () => await Unlock());
            (UnlockCommand as RelayCommand).CANPointer += () => { return IsConnected; };
            XMinusCommand = new RelayCommand(() => XMinus());
            (XMinusCommand as RelayCommand).CANPointer += () => { return IsConnected; };
            XPlusCommand = new RelayCommand(async () => await XPlus());
            (XPlusCommand as RelayCommand).CANPointer += () => { return IsConnected; };
            YPlusCommand = new RelayCommand(async () => await YPlus());
            (YPlusCommand as RelayCommand).CANPointer += () => { return IsConnected; };
            YMinusCommand = new RelayCommand(async () => await YMinus());
            (YMinusCommand as RelayCommand).CANPointer += () => { return IsConnected; };
            ZPlusCommand = new RelayCommand(async () => await ZPlus());
            (ZPlusCommand as RelayCommand).CANPointer += () => { return IsConnected; };
            ZMinusCommand = new RelayCommand(async () => await ZMinus());
            (ZMinusCommand as RelayCommand).CANPointer += () => { return IsConnected; };
            SpindelCommand = new RelayCommand(async () => await Spindel());
            (SpindelCommand as RelayCommand).CANPointer += () => { return IsConnected; };
            ResetCommand = new RelayCommand(async () => await Reset());
            (ResetCommand as RelayCommand).CANPointer += () => { return false; };
            SendLineButtonCommand = new RelayCommand(async () => await SendLine());
            (SendLineButtonCommand as RelayCommand).CANPointer += () => { return IsConnected; };
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

                        if (PresentViewModel.Device == null)
                        {
                            return;
                        }

                        XAMLFiles.Portis p = PortViewModel.PortisList.Find(
                            porti =>
                            PresentViewModel.Device.Interface.Portname == porti.PortName);
                        p.IndicatorColor = Brushes.ForestGreen;
                    }
                    else
                    {
                        PortViewModel.PortisList.ForEach(porti => porti.IndicatorColor = Brushes.Transparent);
                    }

                    (SetZeroCommand as RelayCommand).FireCanExecuteChanged();
                    (HomingCommand as RelayCommand).FireCanExecuteChanged();
                    (UnlockCommand as RelayCommand).FireCanExecuteChanged();
                    (XMinusCommand as RelayCommand).FireCanExecuteChanged();
                    (XPlusCommand as RelayCommand).FireCanExecuteChanged();
                    (YPlusCommand as RelayCommand).FireCanExecuteChanged();
                    (YMinusCommand as RelayCommand).FireCanExecuteChanged();
                    (ZPlusCommand as RelayCommand).FireCanExecuteChanged();
                    (ZMinusCommand as RelayCommand).FireCanExecuteChanged();
                    (SpindelCommand as RelayCommand).FireCanExecuteChanged();
                    (ResetCommand as RelayCommand).FireCanExecuteChanged();
                    (SendLineButtonCommand as RelayCommand).FireCanExecuteChanged();
                    (SendCommand as RelayCommand).FireCanExecuteChanged();

                    break;
                default:
                    break;
            }
        }
    }
}
