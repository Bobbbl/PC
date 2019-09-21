using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PC
{
    public class PresentViewModel
    {
        public List<string> ConsoleText { get; set; }

        public double CurrentX { get; set; }

        public double CurrentY { get; set; }

        public double CurrentZ { get; set; }

        public DispatcherTimer UpdateTimer { get; set; } = new DispatcherTimer();

        public DispatcherTimer PollTimer { get; set; } = new DispatcherTimer();

        public string SelectedPortName { get; set; }

        public int SelectedBaudRate { get; set; }

    }
}
