using System;

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