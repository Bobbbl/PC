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
        public event EventHandler StartUpMessageReceived;
        public event EventHandler AlarmMessageReceived;
        public event EventHandler LimitMessageReceived;
        public event EventHandler AbortDuringCycleMessageReceived;
        public event EventHandler ProbeFailMessageReceived;
        public event EventHandler UnlockNeededMessageReceived;
        public event EventHandler StatusReportMessageReceived;
        public event EventHandler OpenPortFailed; 

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
            CNCMessage rmessage = new CNCMessage() { Message = ""};

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
            SerialInterface.Close();
        }

        public SerialPort SerialInterface { get; set; }

        public SerialGRBLInterface(string PortName, int BaudRate)
        {
            // Default Settings
            System.IO.Ports.SerialPort sport = new System.IO.Ports.SerialPort();
            sport.PortName = PortName;
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
                ToolbarViewModel.IsConnected = true;
            }
            catch (Exception ex)
            {
                if (OpenPortFailed != null)
                    OpenPortFailed(this, new EventArgs());
                ToolbarViewModel.IsConnected = false;
            }

            SerialInterface = sport;
        }
    }
}