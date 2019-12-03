using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;

namespace PC
{
    public abstract class CNCInterface
    {

        public event CNCMessageEventHandler MessageReceived = (s, e) => { };
        public event CNCMessageEventHandler MessageSent = (s, e) => { };

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

        protected Timer PollTimer = new Timer(1000) { AutoReset = true };

        private int _CheckTimeout = 100;
        public int CheckTimeout
        {
            get
            {
                return _CheckTimeout;
            }
            set
            {
                if (value <= 0)
                {
                    PollTimer.Stop();
                }
                else
                {
                    _CheckTimeout = value;
                    PollTimer.Interval = value;
                    PollTimer.Stop();
                    System.Threading.Thread.Sleep(0);
                    PollTimer.Start();
                }

            }
        }

        private bool _MessageQueue = false;
        public bool MessageQueue
        {
            get
            {
                return _MessageQueue;
            }
            set
            {
                if (_MessageQueue != value)
                {
                    _MessageQueue = value;
                    PollTimer.Interval = CheckTimeout;
                    PollTimer.AutoReset = value;
                    PollTimer.Enabled = value;
                }
            }
        }

        public List<CNCMessage> MessageBuffer { get; set; } = new List<CNCMessage>();

        protected List<CNCMessage> SendBuffer { get; set; } = new List<CNCMessage>();

        public abstract void SendMessage(CNCMessage message, bool logging = true);

        public abstract CNCMessage ReceiveMessage(int TimeOut, bool logging = true);

        public abstract Task<CNCMessage> ReceiveMessageAsync(int TimeOut);

        public abstract CNCMessage WaitReceiveMessage(int TimeOut, CNCMessage WaitForMessage, int WaitTimout, bool logging = true);

        public abstract Task<CNCMessage> WaitReceiveMessageAsync(int TimeOut, CNCMessage WaitForMessage, int WaitTimout);

        public abstract CNCMessage WaitReceiveMessageContaining(int timeout, string containing, int waittimout, bool logging = true);

        public abstract CNCMessage WaitReceiveMessageContainingMultible(int timeout, List<string> containing, int waittimout, bool logging = true);

        public abstract Task<CNCMessage> WaitReceiveMessageContainingAsync(int timeout, string containing, int waittimout);

        public abstract void CloseConnection();

        //protected void PollTimer_Elapsed(object sender, ElapsedEventArgs e)
        //{

        //}

        public CNCInterface(bool MQueue)
        {
            MessageQueue = MQueue;

            //PollTimer.Elapsed += PollTimer_Elapsed;
        }


    }
}