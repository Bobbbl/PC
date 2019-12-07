using HelixToolkit.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

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

        public double StreamProgress { get; set; } = 0;

        public string CNCFileContent { get; set; }
        public ObservableCollection<string> CNCFileContentArray { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Point3D> SegmentList { get; set; } = new ObservableCollection<Point3D>();


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


        private GlobalKeyboardHook _GlobalKeyboardHook;


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
            await PresentViewModel.Device.RelativeJogX(-StepSizeJog, FeedRateJog);
        }

        public async Task XPlus()
        {
            await PresentViewModel.Device.RelativeJogX(StepSizeJog, FeedRateJog);
        }

        public async Task YPlus()
        {
            await PresentViewModel.Device.RelativeJogY(StepSizeJog, FeedRateJog);
        }

        public async Task YMinus()
        {
            await PresentViewModel.Device.RelativeJogY(-StepSizeJog, FeedRateJog);
        }

        public async Task ZPlus()
        {
            await PresentViewModel.Device.RelativeJogZ(StepSizeJog, FeedRateJog);
        }

        public async Task ZMinus()
        {
            await PresentViewModel.Device.RelativeJogZ(-StepSizeJog, FeedRateJog);
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

            #region Read File Content

            OpenFileDialog odialog = new OpenFileDialog();
            if (odialog.ShowDialog() == true)
            {
                using (StreamReader sr = File.OpenText(odialog.FileName))
                {
                    CNCFileContent = string.Empty;
                    CNCFileContentArray = new ObservableCollection<string>(Regex.Split(await sr.ReadToEndAsync(), "\r\n|\r|\n"));


                    int e = 0;
                    foreach (string item in CNCFileContentArray)
                    {
                        CNCFileContent += item + '\n';

                        if (e > 1000)
                            break;

                        e++;
                    }


                }
            }

            #endregion

            PlotViewModel.PointList.Clear();

            await Task.Run<Point3DCollection>(() =>
            {

                #region Get Points
                Point3DCollection coll = new Point3DCollection();

                Point3D currentcoor = new Point3D();
                currentcoor.X = 0;
                currentcoor.Y = 0;
                currentcoor.Z = 0;

                foreach (string item in CNCFileContentArray)
                {

                    // Find possible X
                    string xstr = Regex.Match(item, @"[G0|G1] X([0-9]*)").Groups[1].Value;
                    string ystr = Regex.Match(item, @"[G0|G1] Y([0-9]*)").Groups[1].Value;
                    string zstr = Regex.Match(item, @"[G0|G1] Z([0-9]*)").Groups[1].Value;

                    double x, y, z;

                    if (!string.IsNullOrEmpty(xstr) || !string.IsNullOrEmpty(ystr) || !string.IsNullOrEmpty(zstr))
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(xstr))
                            {
                                x = double.Parse(xstr);
                                currentcoor.X = x;
                            }
                            else
                            {
                                x = currentcoor.X;
                            }
                        }
                        catch (FormatException ex)
                        {
                            x = currentcoor.X;
                        }
                        try
                        {
                            if(!string.IsNullOrEmpty(ystr))
                            {
                                y = double.Parse(ystr);
                                currentcoor.Y = y;
                            }
                            else
                            {
                                y = currentcoor.Y;
                            }
                        }
                        catch (FormatException ex)
                        {
                            y = currentcoor.Y;
                        }
                        try
                        {
                            if(!string.IsNullOrEmpty(zstr))
                            {
                                z = double.Parse(zstr);
                                currentcoor.Z = z;
                            }
                            else
                            {
                                z = currentcoor.Z;
                            }

                        }
                        catch (FormatException ex)
                        {
                            z = currentcoor.Z;
                        }

                        Point3D p = new Point3D();
                        p.X = currentcoor.X;
                        p.Y = currentcoor.Y;
                        p.Z = currentcoor.Z;


                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            PlotViewModel.PointList.Add(p);
                        });
                    }

                }

                return coll;

                #endregion
            });



        }

        public async Task Send()
        {
            PresentViewModel.Device.CurrentMode = CommModes.SendMode;
            CNCMessage message0 = new CNCMessage() { Message = CNCFileContentArray[0] };
            CNCMessage answer = new CNCMessage();
            CNCMessage posrequmessage = new CNCMessage();
            CNCMessage posmessage = new CNCMessage();
            CNCMessage rmessage = new CNCMessage() { Message = string.Empty };

            CNC_Device Device = PresentViewModel.Device;
            CNCInterface Interface = Device.Interface;

            long count = 0;
            long maxcount = CNCFileContentArray.Count;
            int okcount = 0;
            float[] r = new float[3];
            int wcount = 0;
            int recd = 0;

            await Task.Run(() =>
            {

                foreach (var item in CNCFileContentArray)
                {
                    message0.Message = item;

                    PresentViewModel.Device.Interface.SendMessage(message0, false);
                    okcount++;

                    posrequmessage = PresentViewModel.Device.Protokoll.GetCurrentXYZMessage();
                    Interface.SendMessage(posrequmessage, false);
                    okcount++;


                    while (okcount > 0)
                    {
                        rmessage = new CNCMessage();
                        rmessage = Interface.ReceiveMessage(100, false);

                        if (rmessage.Message.Contains("ok") || rmessage.Message.Contains("error"))
                        {
                            okcount--;

                        }
                        else if (rmessage.Message.Contains("WPos"))
                        {
                            try
                            {
                                r[0] = Convert.ToSingle(Regex.Match(rmessage.Message, @"(WPos:([-0-9]+.[0-9]+),([-0-9]+.[0-9]+),([-0-9]+.[0-9]))").Groups[2].Value, CultureInfo.InvariantCulture);
                                r[1] = Convert.ToSingle(Regex.Match(rmessage.Message, @"(WPos:([-0-9]+.[0-9]+),([-0-9]+.[0-9]+),([-0-9]+.[0-9]))").Groups[3].Value, CultureInfo.InvariantCulture);
                                r[2] = Convert.ToSingle(Regex.Match(rmessage.Message, @"(WPos:([-0-9]+.[0-9]+),([-0-9]+.[0-9]+),([-0-9]+.[0-9]))").Groups[4].Value, CultureInfo.InvariantCulture);


                                PresentViewModel.Device.CurrentX = r[0];
                                PresentViewModel.Device.CurrentY = r[1];
                                PresentViewModel.Device.CurrentZ = r[2];
                            }
                            catch (Exception ex)
                            {
                                r[0] = (float)PresentViewModel.CurrentX;
                                r[1] = (float)PresentViewModel.CurrentY;
                                r[2] = (float)PresentViewModel.CurrentZ;
                            }


                            recd++;

                        }

                        if (wcount > 2 && recd != 0)
                        {
                            Interface.SendMessage(posrequmessage, false);
                            okcount++;
                            wcount = 0;
                            recd = 0;
                        }
                        wcount++;

                    }




                    StreamProgress = (double)count / (double)maxcount;

                    count++;

                }

            });

            StreamProgress = 1;
            PresentViewModel.Device.CurrentMode = CommModes.DefaultMode;

        }

        #endregion


        #region Constructor ToolbarViewModel
        public ToolbarViewModel()
        {
            SetZeroCommand = new RelayCommand(async () => await SetZero());
            (SetZeroCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            HomingCommand = new RelayCommand(async () => await Homing());
            (HomingCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            UnlockCommand = new RelayCommand(async () => await Unlock());
            (UnlockCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            XMinusCommand = new RelayCommand(async () => await XMinus());
            (XMinusCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            XPlusCommand = new RelayCommand(async () => await XPlus());
            (XPlusCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            YPlusCommand = new RelayCommand(async () => await YPlus());
            (YPlusCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            YMinusCommand = new RelayCommand(async () => await YMinus());
            (YMinusCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            ZPlusCommand = new RelayCommand(async () => await ZPlus());
            (ZPlusCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            ZMinusCommand = new RelayCommand(async () => await ZMinus());
            (ZMinusCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            SpindelCommand = new RelayCommand(async () => await Spindel());
            (SpindelCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            ResetCommand = new RelayCommand(async () => await Reset());
            (ResetCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            SendLineButtonCommand = new RelayCommand(async () => await SendLine());
            (SendLineButtonCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;
            LoadCommand = new RelayCommand(async () => await Load());

            SendCommand = new RelayCommand(async () => await Send());
            (SendCommand as RelayCommand).CANPointer += WhenSendButtonEnabled;

            StaticPropertyChanged += ToolbarViewModel_StaticPropertyChanged;


            // Global Keyboard Hooks
            _GlobalKeyboardHook = new GlobalKeyboardHook();
            _GlobalKeyboardHook.KeyboardPressed += _GlobalKeyboardHook_KeyboardPressed;


        }
        #endregion

        public bool WhenSendButtonEnabled()
        {
            if (PresentViewModel.Device == null)
                return false;

            if (IsConnected && (PresentViewModel.Device.CurrentMode == CommModes.DefaultMode))
                return true;
            else
                return false;
        }

        private void _GlobalKeyboardHook_KeyboardPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp)
            {
                try
                {
                    var focusedelementtype = Keyboard.FocusedElement.GetType();
                    if (focusedelementtype != typeof(System.Windows.Controls.TextBox) &&
                   focusedelementtype != typeof(HelixToolkit.Wpf.CameraController))
                    {
                        if (Keyboard.IsKeyDown(Key.Left))
                        {
                            if (XMinusCommand.CanExecute(null))
                                XMinusCommand.Execute(null);
                        }
                        if (Keyboard.IsKeyDown(Key.Right))
                        {
                            if (XPlusCommand.CanExecute(null))
                                XPlusCommand.Execute(null);
                        }
                        if (Keyboard.IsKeyDown(Key.Down))
                        {
                            if (YMinusCommand.CanExecute(null))
                                YMinusCommand.Execute(null);
                        }
                        if (Keyboard.IsKeyDown(Key.Up))
                        {
                            if (YPlusCommand.CanExecute(null))
                                YPlusCommand.Execute(null);
                        }
                    }
                }
                catch (NullReferenceException ex)
                {
                    return;
                }


            }
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
