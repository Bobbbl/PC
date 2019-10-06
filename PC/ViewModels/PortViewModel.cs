using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PC
{
    public class PortViewModel : BaseViewModel
    {

        public List<string> PortList { get; set; } = new List<string>();

        private DispatcherTimer PortSearchTimer = new DispatcherTimer();


        private void SearchForPortAndAddOrRemove(object sender, EventArgs e)
        {
            List<string> portlist = SerialPort.GetPortNames().ToList();

            var oldports = PortList.Except(portlist).ToList();
            var newports = portlist.Except(PortList).ToList();

            if (oldports.Count == 0 && newports.Count == 0)
            {
                return;
            }

            PortList = portlist;
            OnPropertyChanged(nameof(PortList));

        }


        public PortViewModel()
        {

            PortSearchTimer.Tick += (sender, e) => { SearchForPortAndAddOrRemove(sender, e); };
            PortSearchTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            PortSearchTimer.Start();
        }
    }
}
