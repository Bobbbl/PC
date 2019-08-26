using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PC
{
    public abstract class CNCInterface
    {
        public abstract void SendMessage(CNCMessage message);

        public abstract CNCMessage ReceiveMessage(int TimeOut);
    }
}