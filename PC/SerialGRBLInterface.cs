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

        public SerialGRBLInterface(SerialPort port)
        {
            SerialInterface = port;
        }
    }
}