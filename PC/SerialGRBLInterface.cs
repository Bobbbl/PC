using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace PC
{
    public class SerialGRBLInterface
        : CNCInterface
    {
        public event EventHandler StartUpMessageReceived = (a, b) => { };
        public event EventHandler AlarmMessageReceived = (a, b) => { };
        public event EventHandler LimitMessageReceived = (a, b) => { };
        public event EventHandler AbortDuringCycleMessageReceived = (a, b) => { };
        public event EventHandler ProbeFailMessageReceived = (a, b) => { };
        public event EventHandler UnlockNeededMessageReceived = (a, b) => { };
        public event EventHandler StatusReportMessageReceived = (a, b) => { };
        public event EventHandler OpenPortFailed = (a, b) => { };
        public event EventHandler ClosePortFailed = (a, b) => { };
        public event Action<string, int> PortOpened = (a, b) => { };
        public event Action<string, int> PortClosed = (a, b) => { };
        public void FirePortOpened()
        {
            PortOpened(Portname, SerialInterface.BaudRate);
        }
        public void FirePortClosed(string portname, int baudrate)
        {
            PortClosed(Portname, SerialInterface.BaudRate);
        }

        public CNCMessage LastMessageReceived { get; set; }

        public override CNCMessage ReceiveMessage(int TimeOut = 100)
        {
            CNCMessage rmessage = new CNCMessage();

            LastMessageReceived = null;

            try
            {
                SerialInterface.ReadTimeout = TimeOut;
                rmessage.Message = SerialInterface.ReadLine();
                LastMessageReceived = rmessage;
                OnMessageReceived(this, rmessage);
            }
            catch (TimeoutException ex)
            {
                rmessage.Message = "TIMEOUT";
            }

            return rmessage;
        }

        public override CNCMessage WaitReceiveMessage(int TimeOut = 100, CNCMessage WaitForMessage = null, int WaitTimeout = 0)
        {
            CNCMessage rmessage = new CNCMessage() { Message = "" };

            LastMessageReceived = null;

            if (WaitTimeout == 0)
            {
                try
                {
                    SerialInterface.ReadTimeout = TimeOut;
                    rmessage.Message = SerialInterface.ReadLine();
                    LastMessageReceived = rmessage;
                    OnMessageReceived(this, rmessage);
                }
                catch (TimeoutException ex)
                {
                    rmessage.Message = "TIMEOUT";
                }

            }
            else
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (!rmessage.Message.Contains(WaitForMessage.Message))
                {
                    try
                    {
                        SerialInterface.ReadTimeout = TimeOut;
                        rmessage.Message = SerialInterface.ReadLine();
                        LastMessageReceived = rmessage;
                        OnMessageReceived(this, rmessage);
                    }
                    catch (TimeoutException ex)
                    {
                        rmessage.Message = "TIMEOUT";
                    }

                    if (sw.ElapsedMilliseconds >= WaitTimeout)
                        return rmessage;
                }
            }




            return rmessage;
        }

        public override void SendMessage(CNCMessage message)
        {
            if (AutoPoll)
            {
                MessageBuffer.Add(message);
                OnMessageSent(this, message);
            }
            else
            {
                SerialInterface.WriteLine(message.Message);
            }
        }

        public override void CloseConnection()
        {
            try
            {
                SerialInterface.Close();
                PortClosed(Portname, SerialInterface.BaudRate);
            }
            catch (Exception)
            {
                ClosePortFailed(this, new EventArgs());
            }
        }

        public SerialPort SerialInterface { get; set; }

        public SerialGRBLInterface(string PortName, int BaudRate)
        {
            // Default Settings
            System.IO.Ports.SerialPort sport = new System.IO.Ports.SerialPort();
            this.Portname = PortName;
            sport.PortName = this.Portname;
            sport.BaudRate = BaudRate;
            sport.Parity = System.IO.Ports.Parity.None;
            sport.DataBits = 8;
            sport.StopBits = System.IO.Ports.StopBits.One;
            sport.Handshake = System.IO.Ports.Handshake.None;
            // Serial Port Time Outs
            sport.WriteTimeout = 200;
            sport.ReadTimeout = 200;
            try
            {
                sport.Open();
                PortOpened(Portname, BaudRate);

            }
            catch (Exception ex)
            {
                OpenPortFailed(this, new EventArgs());
            }

            SerialInterface = sport;
        }
    }
}