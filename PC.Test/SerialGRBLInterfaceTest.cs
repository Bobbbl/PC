using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC.Test
{
    public class SerialGRBLInterfaceTest
    {
        public SerialGRBLInterface GRBLInterface { get; set; }

        public string PortName { get; set; }

        public int BaudRate { get; set; }

        public SerialGRBLInterfaceTest()
        {
            GRBLInterface = new SerialGRBLInterface(PortName, BaudRate);
        }
    }
}
