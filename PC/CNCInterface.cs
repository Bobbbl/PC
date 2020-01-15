using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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

        public void AddToSendReceiveBuffer(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            SendReceiveBuffer.Add(message);

            if (SendReceiveBuffer.Count > 1000)
            {
                for (int i = 0; i < 100; i++)
                {
                    SendReceiveBuffer.RemoveAt(0);
                }
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
        public abstract void SendMessage(CNCMessage message, bool logging = true);

        public abstract CNCMessage ReceiveMessage(int TimeOut, bool logging = true);
        public abstract Task<CNCMessage> ReceiveMessageAsync(int TimeOut);

        public abstract CNCMessage WaitReceiveMessage(int TimeOut, CNCMessage WaitForMessage, int WaitTimout, bool logging = true);
        public abstract Task<CNCMessage> WaitReceiveMessageAsync(int TimeOut, CNCMessage WaitForMessage, int WaitTimout);

        public abstract CNCMessage WaitReceiveMessageContaining(int timeout, string containing, int waittimout, bool logging = true);
        public abstract CNCMessage WaitReceiveMessageContainingMultible(int timeout, List<string> containing, int waittimout, bool logging = true);
        public abstract Task<CNCMessage> WaitReceiveMessageContainingAsync(int timeout, string containing, int waittimout);


        public abstract void CloseConnection();



        public CNCInterface()
        {

        }
    }
}