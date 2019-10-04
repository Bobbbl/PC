using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC
{
    public class PortisEventsArgs : CNCEventsArgs
    {
        public string PortName { get; set; }

        public int BaudRate { get; set; }
    }
}
