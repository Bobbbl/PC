using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PC
{
    public class CNCMessage
    {


        public string Message { get; set; } = String.Empty;

        public bool GRBLCommand { get; set; }

        public void AppendCommand(string c)
        {
            Message += c;
        }


    }
}