using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Timers;

namespace PC
{
    public abstract class CNCInterface
    {

        public event CNCMessageEventHandler MessageReceived;
        public event CNCMessageEventHandler MessageSent;

        public string Portname { get; set; } = "";

        private ObservableCollection<string> _SendReceiveBuffer = new ObservableCollection<string>();
        public ObservableCollection<string> SendReceiveBuffer
        {
            get
            {
                return _SendReceiveBuffer;
            }

            set
            {
                _SendReceiveBuffer = value;
            }
        }

        public void OnMessageReceived(object sender, CNCMessage message)
        {
            if (MessageReceived != null)
            {
                MessageReceived(sender, new CNCEventsArgs() { Message = message });
            }
        }

        public void OnMessageSent(object sender, CNCMessage message)
        {
            MessageSent(sender, new CNCEventsArgs() { Message = message });
        }

        private Timer PollTimer = new Timer(1000) { AutoReset = true };

        private int _PollTimeout = 1000;
        public int PollTimeout
        {
            get
            {
                return _PollTimeout;
            }
            set
            {
                if (value < 0)
                    return;

                _PollTimeout = value;
                PollTimer.Interval = value;
                PollTimer.Stop();
                System.Threading.Thread.Sleep(0);
                PollTimer.Start();
            }
        }

        private bool _AutoPoll = false;
        public bool AutoPoll
        {
            get
            {
                return _AutoPoll;
            }
            set
            {
                PollTimer.Interval = PollTimeout;
                PollTimer.AutoReset = value;
                PollTimer.Enabled = value;
            }
        }

        public List<CNCMessage> MessageBuffer { get; set; } = new List<CNCMessage>();
        public abstract void SendMessage(CNCMessage message);

        public abstract CNCMessage ReceiveMessage(int TimeOut);

        public abstract CNCMessage WaitReceiveMessage(int TimeOut, CNCMessage WaitForMessage, int WaitTimout);

        public abstract void CloseConnection();



        public CNCInterface()
        {

        }
    }
}