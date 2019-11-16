using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC
{
    public class SerialGRBLInterface
        : CNCInterface
    {
        public event EventHandler StartUpMessageReceived = (a, b) => { };
        public event EventHandler AlarmMessageReceived = (a, b) => { };
        public event EventHandler LimitMessageReceived = (a, b) => { };
        public event EventHandler AbortDuringCycleMessageReceived = (a, b) => { };
        public event EventHandler ProbeFailMessageReceived = (a, b) => { };
        public event EventHandler UnlockNeededMessageReceived = (a, b) => { };
        public event EventHandler StatusReportMessageReceived = (a, b) => { };
        public event EventHandler OpenPortFailed = (a, b) => { };
        public event EventHandler ClosePortFailed = (a, b) => { };
        public event Action<string, int> PortOpened = (a, b) => { };
        public event Action<string, int> PortClosed = (a, b) => { };
        public void FirePortOpened()
        {
            PortOpened(Portname, SerialInterface.BaudRate);
        }
        public void FirePortClosed(string portname, int baudrate)
        {
            PortClosed(Portname, SerialInterface.BaudRate);
        }


        public CNCMessage LastMessageReceived { get; set; }

        public override async Task<CNCMessage> ReceiveMessageAsync(int timeout = 100)
        {
            CNCMessage rmessage = new CNCMessage();

            LastMessageReceived = null;

            try
            {
                SerialInterface.ReadTimeout = timeout;
                var tmp = new byte[1];
                byte recbyte = 0x00;
                string recstring = string.Empty;
                while (!recstring.Contains("\n") && !recstring.Contains("\r"))
                {
                    await SerialInterface.BaseStream.ReadAsync(tmp, 0, 1);
                    recstring += Encoding.ASCII.GetString(tmp);
                }
                rmessage.Message = BitConverter.ToString(tmp).Trim();
                LastMessageReceived = rmessage;
                OnMessageReceived(this, rmessage);
            }
            catch (TimeoutException ex)
            {
                rmessage.Message = "TIMEOUT";
                return rmessage;
            }
            catch (InvalidOperationException ex)
            {
                rmessage.Message = ex.Message;
                CloseConnection();
                return rmessage;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                rmessage.Message = ex.Message;
                return rmessage;
            }


            SendReceiveBuffer.Add(rmessage.Message);
            return rmessage;
        }

        public override CNCMessage ReceiveMessage(int TimeOut = 100, bool logging = true)
        {
            CNCMessage rmessage = new CNCMessage();

            LastMessageReceived = null;

            try
            {
                SerialInterface.ReadTimeout = TimeOut;
                if (SerialInterface.BytesToRead >= 0)
                    rmessage.Message = SerialInterface.ReadLine();
                LastMessageReceived = rmessage;
                OnMessageReceived(this, rmessage);
            }
            catch (TimeoutException ex)
            {
                rmessage.Message = "TIMEOUT";
                return rmessage;
            }
            catch (InvalidOperationException ex)
            {
                rmessage.Message = ex.Message;
                CloseConnection();
                if (logging)
                    SendReceiveBuffer.Add(rmessage.Message);
                return rmessage;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                rmessage.Message = ex.Message;
                if (logging)
                    SendReceiveBuffer.Add(rmessage.Message);
                return rmessage;
            }
            catch (IndexOutOfRangeException ex)
            {
                // This happens if while this thread is reading bytes from SerialPort, a other
                // thread is reading the bytes from port. this can then result in an -1 data pointer
                // (SerialPort is not thread save). 
                // The good news are; this only means the message was already read from port and nothing
                // else is to do.
                Debugger.Break();
                rmessage.Message = "-";
                return rmessage;
            }
            catch (OverflowException ex)
            {
                // This happens if while this thread is reading bytes from SerialPort, a other
                // thread is reading the bytes from port. this can then result in an -1 data pointer
                // (SerialPort is not thread save). 
                // The good news are; this only means the message was already read from port and nothing
                // else is to do.
                Debugger.Break();
                rmessage.Message = "-";
                return rmessage;
            }


            if (logging)
                SendReceiveBuffer.Add(rmessage.Message);
            return rmessage;
        }

        public override CNCMessage WaitReceiveMessageContaining(int timeout, string containing, int waittimout, bool logging = true)
        {
            CNCMessage rmessage = new CNCMessage() { Message = "" };

            LastMessageReceived = null;

            if (waittimout == 0)
            {
                try
                {
                    SerialInterface.ReadTimeout = timeout;
                    rmessage = ReceiveMessage(timeout, logging: logging);
                    LastMessageReceived = rmessage;
                    OnMessageReceived(this, rmessage);
                }
                catch (TimeoutException ex)
                {
                    rmessage.Message = "TIMEOUT";
                }

            }
            else
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (!rmessage.Message.Contains(containing))
                {
                    try
                    {
                        SerialInterface.ReadTimeout = timeout;
                        rmessage = ReceiveMessage(timeout, logging: logging);
                        LastMessageReceived = rmessage;
                        OnMessageReceived(this, rmessage);
                    }
                    catch (TimeoutException ex)
                    {
                        rmessage.Message = "TIMEOUT";
                    }

                    if (sw.ElapsedMilliseconds >= waittimout)
                        return rmessage;
                }
            }




            return rmessage;
        }

        public override async Task<CNCMessage> WaitReceiveMessageContainingAsync(int timeout, string containing, int waittimout)
        {
            CNCMessage rmessage = new CNCMessage() { Message = "" };

            LastMessageReceived = null;

            if (waittimout == 0)
            {
                try
                {
                    SerialInterface.ReadTimeout = timeout;
                    rmessage = await ReceiveMessageAsync(timeout);
                    LastMessageReceived = rmessage;
                    OnMessageReceived(this, rmessage);
                }
                catch (TimeoutException ex)
                {
                    rmessage.Message = "TIMEOUT";
                }

            }
            else
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (!rmessage.Message.Contains(containing))
                {
                    try
                    {
                        SerialInterface.ReadTimeout = timeout;
                        rmessage = await ReceiveMessageAsync(timeout);
                        LastMessageReceived = rmessage;
                        OnMessageReceived(this, rmessage);
                    }
                    catch (TimeoutException ex)
                    {
                        rmessage.Message = "TIMEOUT";
                    }

                    if (sw.ElapsedMilliseconds >= waittimout)
                        return rmessage;
                }
            }




            return rmessage;
        }


        public override CNCMessage WaitReceiveMessage(int TimeOut = 100, CNCMessage WaitForMessage = null, int WaitTimeout = 0, bool logging = true)
        {
            CNCMessage rmessage = new CNCMessage() { Message = "" };

            LastMessageReceived = null;

            if (WaitTimeout == 0)
            {
                try
                {
                    SerialInterface.ReadTimeout = TimeOut;
                    rmessage = ReceiveMessage(TimeOut, logging: logging);
                    LastMessageReceived = rmessage;
                    OnMessageReceived(this, rmessage);
                }
                catch (TimeoutException ex)
                {
                    rmessage.Message = "TIMEOUT";
                }

            }
            else
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (!rmessage.Message.Contains(WaitForMessage.Message))
                {
                    try
                    {
                        SerialInterface.ReadTimeout = TimeOut;
                        rmessage = ReceiveMessage(TimeOut, logging: logging);
                        LastMessageReceived = rmessage;
                        OnMessageReceived(this, rmessage);
                    }
                    catch (TimeoutException ex)
                    {
                        rmessage.Message = "TIMEOUT";
                    }

                    if (sw.ElapsedMilliseconds >= WaitTimeout)
                        return rmessage;
                }
            }




            return rmessage;
        }
        public override async Task<CNCMessage> WaitReceiveMessageAsync(int TimeOut = 100, CNCMessage WaitForMessage = null, int WaitTimeout = 0)
        {
            CNCMessage rmessage = new CNCMessage() { Message = "" };

            LastMessageReceived = null;

            if (WaitTimeout == 0)
            {
                try
                {
                    SerialInterface.ReadTimeout = TimeOut;
                    rmessage = await ReceiveMessageAsync(TimeOut);
                    LastMessageReceived = rmessage;
                    OnMessageReceived(this, rmessage);
                }
                catch (TimeoutException ex)
                {
                    rmessage.Message = "TIMEOUT";
                }

            }
            else
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (!rmessage.Message.Contains(WaitForMessage.Message))
                {
                    try
                    {
                        SerialInterface.ReadTimeout = TimeOut;
                        rmessage = await ReceiveMessageAsync(TimeOut);
                        LastMessageReceived = rmessage;
                        OnMessageReceived(this, rmessage);
                    }
                    catch (TimeoutException ex)
                    {
                        rmessage.Message = "TIMEOUT";
                    }

                    if (sw.ElapsedMilliseconds >= WaitTimeout)
                        return rmessage;
                }
            }




            return rmessage;
        }

        public override void SendMessage(CNCMessage message, bool logging = true)
        {
            if (AutoPoll)
            {
                MessageBuffer.Add(message);
                OnMessageSent(this, message);
            }
            else
            {
                try
                {
                    SerialInterface.WriteLine(message.Message);
                }
                catch (InvalidOperationException ex)
                {
                    SendReceiveBuffer.Add(ex.Message);
                    return;
                }
            }

            if (logging)
                SendReceiveBuffer.Add(message.Message);
        }


        public override void CloseConnection()
        {
            try
            {
                SerialInterface.Close();
                PortClosed(Portname, SerialInterface.BaudRate);
            }
            catch (Exception)
            {
                ClosePortFailed(this, new EventArgs());
            }
        }

        public SerialPort SerialInterface { get; set; }

        public SerialGRBLInterface(string PortName, int BaudRate)
        {
            // Default Settings
            System.IO.Ports.SerialPort sport = new System.IO.Ports.SerialPort();
            this.Portname = PortName;
            sport.PortName = this.Portname;
            sport.BaudRate = BaudRate;
            sport.Parity = System.IO.Ports.Parity.None;
            sport.DataBits = 8;
            sport.StopBits = System.IO.Ports.StopBits.One;
            sport.Handshake = System.IO.Ports.Handshake.None;
            // Serial Port Time Outs
            sport.WriteTimeout = 200;
            sport.ReadTimeout = 200;
            try
            {
                sport.Open();
                PortOpened(Portname, BaudRate);

            }
            catch (Exception ex)
            {
                OpenPortFailed(this, new EventArgs());
            }

            SerialInterface = sport;
        }
    }
}