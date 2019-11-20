namespace PC
{
    public class PortisEventsArgs : CNCEventsArgs
    {
        public string PortName { get; set; }

        public int BaudRate { get; set; }
    }
}
