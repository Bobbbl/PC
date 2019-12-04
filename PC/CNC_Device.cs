using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace PC
{
    public class CNC_Device : INotifyPropertyChanged
    {
        /// <summary>
        /// Implements INotifyPropertyChanged - Interface
        /// </summary>
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnNotifyPropertyChanged(string PropName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropName));
            }
        }

        #endregion

        /// <summary>
        /// Holds always the current x coordinate at any given time
        /// 
        /// Returns:
        /// A float which represents the current X-Coordinate at any given time
        /// <value>
        /// float
        /// </value>
        public float CurrentX { get; set; }

        /// <summary>
        /// Holds always the current y coordinate at any given time
        /// 
        /// Returns:
        /// A float which represents the current X-Coordinate at any given time
        /// </summary>
        public float CurrentY { get; set; }

        /// <summary>
        /// Holds always the current z coordinate at any given time
        /// 
        /// Returns:
        /// A float which represents the current X-Coordinate at any given time
        /// </summary>
        public float CurrentZ { get; set; }

        /// <summary>
        /// Holds always the current spindle rpms at any given time
        /// 
        /// Returns:
        /// A float which represents the spindle rpms at any given time
        /// </summary>
        public float CurrentSpindleRPM { get; set; }

        /// <summary>
        /// Holds always the current coordinate of the tool as {x,y,z}
        /// 
        /// Returns:
        /// A Point3D - Class which holds the current coordinate of the tool at any given moment.
        /// </summary>
        public Point3D CurrentCoordinate { get; set; }

        /// <summary>
        /// Holds the current feed of the device
        /// 
        /// Returns:
        /// A float which represents the current feed of the tool. Don't confuse it with the current
        /// acceleration of the tool!
        /// </summary>
        public float CurrentFeed { get; set; }

        /// <summary>
        /// The current mode the system is in
        /// </summary>
        private CommModes _CurrentMode = CommModes.DefaultMode;
        public CommModes CurrentMode
        {
            get
            {
                return _CurrentMode;
            }
            set
            {
                if (_CurrentMode != value)
                {
                    _CurrentMode = value;

                    switch (value)
                    {
                        case CommModes.DefaultMode:
                            RefreshInterval = _OldRefreshInterval;
                            break;
                        case CommModes.SendMode:
                            RefreshInterval = 0;
                            break;
                        default:
                            break;
                    }

                    
                }
            }
        }

        private DispatcherTimer PositionRefreshTimer { get; set; } = new DispatcherTimer();

        private int _OldRefreshInterval;
        private int _RefreshInterval;
        public int RefreshInterval
        {
            get { return _RefreshInterval; }
            set
            {
                if (value != _RefreshInterval && value >= 0)
                    _RefreshInterval = value;

                if (value <= 0)
                    PositionRefreshTimer.Stop();
                else
                {
                    PositionRefreshTimer.Stop();
                    PositionRefreshTimer.Interval = new TimeSpan(0, 0, 0, 0, value); ;
                    _OldRefreshInterval = value;
                    PositionRefreshTimer.Start();
                }
            }
        }


        public ObservableCollection<string> SendReceiveBuffer
        {
            get
            {
                return Interface.SendReceiveBuffer;
            }
            set
            {
                Interface.SendReceiveBuffer = value;
            }
        }

        /// <summary>
        /// Private Function which is deriving the current modified feed in the cnc-controller
        /// </summary>
        /// <returns></returns>
        public async Task<float> GetCurrentFeed()
        {
            float r = 0;
            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetCurrentFeedMessage();
                Interface.SendMessage(message);
                CNCMessage output = Interface.ReceiveMessage(100);
                var tmp = Regex.Match(output.Message, @"F([0-9]{1,10})").Groups[1].Value;
                r = Convert.ToSingle(tmp, CultureInfo.InvariantCulture);
            });

            return r;
        }

        /// <summary>
        /// Private Function which is deriving the current X in the cnc-controller
        /// </summary>
        /// <returns></returns>
        public async Task<float> GetCurrentX()
        {
            float r = 0;
            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetCurrentXMessage();
                Interface.SendMessage(message);

                CNCMessage output = Interface.WaitReceiveMessageContaining(100, "WPos", 1000);
                var tmp = Regex.Match(output.Message, @"(WPos:([-0-9]+.[0-9]+),([-0-9]+.[0-9]+),([-0-9]+.[0-9]))").Groups[2].Value;
                try
                {
                    r = Convert.ToSingle(tmp, CultureInfo.InvariantCulture);
                }
                catch (FormatException ex)
                {
                    r = 0;
                }
            });

            return r;
        }

        /// <summary>
        /// Private Function which is deriving the Y in the cnc-controller
        /// </summary>
        /// <returns></returns>
        public async Task<float> GetCurrentY()
        {
            float r = 0;
            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetCurrentZMessage();
                Interface.SendMessage(message);
                CNCMessage output = Interface.ReceiveMessage(100);
                var tmp = Regex.Match(output.Message, @"(WPos:([-0-9]+.[0-9]+),([-0-9]+.[0-9]+),([-0-9]+.[0-9]+))").Groups[3].Value;
                r = Convert.ToSingle(tmp, CultureInfo.InvariantCulture);
            });

            return r;
        }

        /// <summary>
        /// Private Function which is deriving the current Z in the cnc-controller
        /// </summary>
        /// <returns></returns>
        public async Task<float> GetCurrentZ()
        {
            float r = 0;
            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetCurrentZMessage();
                Interface.SendMessage(message);
                CNCMessage output = Interface.ReceiveMessage(100);
                var tmp = Regex.Match(output.Message, @"(WPos:([-0-9]+.[0-9]+),([-0-9]+.[0-9]+),([-0-9]+.[0-9]+))").Groups[4].Value;
                r = Convert.ToSingle(tmp, CultureInfo.InvariantCulture);
            });

            return r;
        }

        public async Task<float[]> GetCurrentXYZ()
        {
            float[] r = new float[3];
            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetCurrentXYZMessage();
                if (ConsoleViewModel.Verbose)
                {
                    Interface.SendMessage(message);
                }
                else
                {
                    Interface.SendMessage(message, false);
                }

                CNCMessage output;
                CNCMessage outokerror;
                List<string> outarray = new List<string>();
                outarray.Add("ok");
                outarray.Add("error");


                if (ConsoleViewModel.Verbose)
                {
                    output = Interface.WaitReceiveMessageContaining(100, "WPos", 2000);
                    outokerror = Interface.WaitReceiveMessageContainingMultible(100, outarray, 2000);
                }
                else
                {
                    output = Interface.WaitReceiveMessageContaining(100, "WPos", 2000, logging: false);
                    outokerror = Interface.WaitReceiveMessageContainingMultible(100, outarray, 2000, logging: false);
                }


                try
                {
                    r[0] = Convert.ToSingle(Regex.Match(output.Message, @"(WPos:([-0-9]+.[0-9]+),([-0-9]+.[0-9]+),([-0-9]+.[0-9]))").Groups[2].Value, CultureInfo.InvariantCulture);
                    r[1] = Convert.ToSingle(Regex.Match(output.Message, @"(WPos:([-0-9]+.[0-9]+),([-0-9]+.[0-9]+),([-0-9]+.[0-9]))").Groups[3].Value, CultureInfo.InvariantCulture);
                    r[2] = Convert.ToSingle(Regex.Match(output.Message, @"(WPos:([-0-9]+.[0-9]+),([-0-9]+.[0-9]+),([-0-9]+.[0-9]))").Groups[4].Value, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    r[0] = CurrentX;
                    r[1] = CurrentY;
                    r[2] = CurrentZ;
                }

            });

            return r;
        }


        public async Task<CNCMessage> SendKillAlarm()
        {
            CNCMessage tmp = null;
            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetKillAlarmMessage();
                Interface.SendMessage(message);
                tmp = Interface.ReceiveMessage(100);
            });
            return tmp;
        }

        public async Task<CNCMessage> SendSoftReset()
        {
            CNCMessage tmp = null;
            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetSoftResetMessage();
                Interface.SendMessage(message);
                tmp = Interface.ReceiveMessage(100);
            });

            return tmp;
        }

        public async Task<CNCMessage> SendSetZero()
        {
            CNCMessage tmp = null;
            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetSetZeroMessage();
                Interface.SendMessage(message);
                tmp = Interface.ReceiveMessage(100);
            });

            return tmp;
        }

        public async Task<CNCMessage> JogX(double X, double Feed)
        {
            CNCMessage tmp = null;

            await Task.Run(() =>


            {
                CNCMessage message = Protokoll.GetJogByXMessage(X, Feed);
                CNCMessage t = new CNCMessage() { Message = "ok" };
                Interface.SendMessage(message);
                tmp = Interface.WaitReceiveMessage(100, t, 1000);
            });


            return tmp;
        }

        public async Task<CNCMessage> JogY(double Y, double Feed)
        {
            CNCMessage tmp = null;
            await Task.Run(() =>
            {

                GRBLProtokoll pr = new GRBLProtokoll();
                CNCMessage m = pr.GetCurrentFeedMessage();
                //CNCMessage start = new CNCMessage() { Message = "Grbl 1.1g ['$' for help]" };
                //var an = Interface.WaitReceiveMessage(100, start, 2000);

                CNCMessage message = Protokoll.GetJogByYMessage(Y, Feed);
                Interface.SendMessage(message);
                CNCMessage t = new CNCMessage() { Message = "ok" };
                tmp = Interface.WaitReceiveMessage(100, t, 1000);
            });

            return tmp;
        }

        public async Task<CNCMessage> JogZ(double Z, double Feed)
        {
            CNCMessage tmp = null;
            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetJogByYMessage(Z, Feed);
                Interface.SendMessage(message);
                tmp = Interface.ReceiveMessage(100);
            });

            return tmp;
        }

        public async Task<CNCMessage> MoveByX(double X, double F)
        {
            CNCMessage tmp = null;
            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetMoveByXMessage(X, F);
                Interface.SendMessage(message);
                CNCMessage t = new CNCMessage() { Message = "ok" };
                tmp = (Interface as SerialGRBLInterface).WaitReceiveMessage(100, t, 1000);
            });
            return tmp;
        }

        public async Task<CNCMessage> MoveByY(double Y, double F)
        {
            CNCMessage tmp = null;
            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetMoveByXMessage(Y, F);
                Interface.SendMessage(message);
                CNCMessage t = new CNCMessage() { Message = "ok" };
                tmp = Interface.WaitReceiveMessage(100, t, 1000);
            });
            return tmp;
        }

        public async Task<CNCMessage> MoveByZ(double Z, double F)
        {
            CNCMessage tmp = null;
            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetMoveByXMessage(Z, F);
                Interface.SendMessage(message);
                CNCMessage t = new CNCMessage() { Message = "ok" };
                tmp = Interface.WaitReceiveMessage(100, t, 1000);
            });
            return tmp;
        }

        public async Task<CNCMessage> MoveByXYZ(double X, double Y, double Z, double F)
        {
            CNCMessage tmp = null;
            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetMoveByXYZMessage(X, Y, Z, F);
                Interface.SendMessage(message);
                CNCMessage t = new CNCMessage() { Message = "ok" };
                tmp = Interface.WaitReceiveMessage(100, t, 1000);
            });
            return tmp;
        }

        public async Task<CNCMessage> RelativeJogX(double X, double F)
        {
            CNCMessage tmp = null;

            CNCMessage message = Protokoll.GetRelativeJogByXMessage(X, F);
            Interface.SendMessage(message);
            CNCMessage t = new CNCMessage() { Message = "ok" };

            await Task.Run(() =>
            {
                tmp = Interface.WaitReceiveMessage(100, t, 1000);

            });

            return tmp;
        }

        public async Task<CNCMessage> RelativeJogY(double Y, double F)
        {
            CNCMessage tmp = null;

            await Task.Run(() =>
            {

                CNCMessage message = Protokoll.GetRelativeJogByYMessage(Y, F);
                Interface.SendMessage(message);
                CNCMessage t = new CNCMessage() { Message = "ok" };
                tmp = Interface.WaitReceiveMessage(100, t, 1000);

            });

            return tmp;
        }

        public async Task<CNCMessage> RelativeJogZ(double Z, double F)
        {
            CNCMessage tmp = null;

            await Task.Run(() =>
            {

                CNCMessage message = Protokoll.GetRelativeJogByZMessage(Z, F);
                Interface.SendMessage(message);
                CNCMessage t = new CNCMessage() { Message = "ok" };
                tmp = Interface.WaitReceiveMessage(100, t, 1000);

            });

            return tmp;
        }

        public async Task<CNCMessage> RelativeJogXYZ(double X, double Y, double Z, double F)
        {
            CNCMessage tmp = null;

            await Task.Run(() =>
            {

                CNCMessage message = Protokoll.GetRelativeJogByXYZMessage(X, Y, Z, F);
                Interface.SendMessage(message);
                CNCMessage t = new CNCMessage() { Message = "ok" };
                tmp = Interface.WaitReceiveMessage(100, t, 1000);

            });

            return tmp;
        }

        public async Task<CNCMessage> DoHoming()
        {
            CNCMessage tmp = null;

            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetHomingMessage();
                Interface.SendMessage(message);
                CNCMessage t = new CNCMessage() { Message = "ok" };
                tmp = Interface.WaitReceiveMessage(100, t, 1000);

            });

            return tmp;
        }

        public async Task<CNCMessage> KillAlarm()
        {
            CNCMessage tmp = null;

            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetKillAlarmMessage();
                Interface.SendMessage(message);
                CNCMessage t = new CNCMessage() { Message = "ok" };
                tmp = Interface.WaitReceiveMessage(100, t, 1000);

            });

            return tmp;
        }

        public async Task<CNCMessage> SetSpindleSpeed(double RPM, string Direction)
        {
            CNCMessage tmp = null;

            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetSpindelSetRPMMessage(RPM, Direction);
                Interface.SendMessage(message);
                CNCMessage t = new CNCMessage() { Message = "ok" };
                tmp = Interface.WaitReceiveMessage(100, t, 1000);
            });

            return tmp;
        }

        /// <summary>
        /// This array holds two messages. "ok" and "error". This are the two messages which are by default returned
        /// by GRBL. If one of this two is received, the handler will not wait any longer for a other messages.
        /// </summary>
        private List<string> _StandardWaitForArray = new List<string>();
        public async Task<CNCMessage> SendCustomMessage(CNCMessage message, int WaitTimeout = 1000)
        {
            CNCMessage tmp = null;
            await Task.Run(() =>
            {
                Interface.SendMessage(message);
                tmp = Interface.WaitReceiveMessageContainingMultible(100, _StandardWaitForArray, WaitTimeout);
            });

            return tmp;
        }


        /// <summary>
        /// Thish holds the current Interface. The Interface can be of different
        /// and this way it can connected via SerialPort, USB etc.
        /// </summary>
        public CNCInterface Interface { get; set; }

        /// <summary>
        /// Thish holds the current Protokoll for providing CNCMessages
        /// </summary>
        public CNCProtokoll Protokoll { get; set; }

        public MachineStates State
        {
            get => default;
            set
            {
            }
        }



        #region Constructor
        public CNC_Device(CNCInterface IFace, CNCProtokoll protokoll)
        {
            Interface = IFace;
            Protokoll = protokoll;
            PositionRefreshTimer.Tick += async (s, e) => await PositionRefreshTimer_Tick(s, e);

            _StandardWaitForArray.Add("ok");
            _StandardWaitForArray.Add("error");

        }


        ~CNC_Device()
        {
        }
        #endregion

        private async Task PositionRefreshTimer_Tick(object sender, EventArgs e)
        {
            PositionRefreshTimer.Stop();
            var farray = await GetCurrentXYZ();

            CurrentX = farray[0];
            CurrentY = farray[1];
            CurrentZ = farray[2];

            if (RefreshInterval > 0)
                PositionRefreshTimer.Start();
        }
    }
}