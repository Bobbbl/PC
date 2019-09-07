using System;
using System.Collections.Generic;
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

        public override CNCMessage ReceiveMessage(int TimeOut)
        {
            throw new NotImplementedException();
        }

        public override void SendMessage(CNCMessage message)
        {
            throw new NotImplementedException();
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
            sport.WriteTimeout = 500;
            sport.ReadTimeout = 500;
            sport.Open();

            SerialInterface = sport;
        }
    }
}