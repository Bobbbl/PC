using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PC
{
    public class PresentViewModel : BaseViewModel
    {
        public List<string> ConsoleText { get; set; }

        public static double CurrentX { get; set; }

        public static double CurrentY { get; set; }

        public static double CurrentZ { get; set; }

        public DispatcherTimer UpdateTimer { get; set; } = new DispatcherTimer();

        public DispatcherTimer PollTimer { get; set; } = new DispatcherTimer();

        public string SelectedPortName { get; set; }

        public int SelectedBaudRate { get; set; }

    }
}
