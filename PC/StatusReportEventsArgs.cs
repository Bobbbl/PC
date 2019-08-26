using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace PC
{
    public class StatusReportEventsArgs : CNCEventsArgs
    {
        public Point3D MPos { get; set; }

        public int Buf { get; set; }

        public int RX { get; set; }

        public MachineStates State { get; set; }
    }
}