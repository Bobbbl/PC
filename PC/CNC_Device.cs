using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PC
{
    public class CNC_Device
    {
        /// <summary>
        /// Implements INotifyPropertyChanged - Interface
        /// </summary>
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnNotifyPropertyChanged(string PropName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropName));
            }
        }

        #endregion

        /// <summary>
        /// Holds always the current x coordinate at any given time
        /// 
        /// Returns:
        /// A float which represents the current X-Coordinate at any given time
        /// <value>
        /// float
        /// </value>
        public float CurrentX { get; set; }

        /// <summary>
        /// Holds always the current y coordinate at any given time
        /// 
        /// Returns:
        /// A float which represents the current X-Coordinate at any given time
        /// </summary>
        public float CurrentY { get; set; }

        /// <summary>
        /// Holds always the current z coordinate at any given time
        /// 
        /// Returns:
        /// A float which represents the current X-Coordinate at any given time
        /// </summary>
        public float CurrentZ { get; set; }

        /// <summary>
        /// Holds always the current coordinate of the tool as {x,y,z}
        /// 
        /// Returns:
        /// A Point3D - Class which holds the current coordinate of the tool at any given moment.
        /// </summary>
        public Point3D CurrentCoordinate { get; set; }

        /// <summary>
        /// Holds the current feed of the device
        /// 
        /// Returns:
        /// A float which represents the current feed of the tool. Don't confuse it with the current
        /// acceleration of the tool!
        /// </summary>
        public float CurrentFeed { get; set; }

        /// <summary>
        /// Private Function which is deriving the current modified feed in the cnc-controller
        /// </summary>
        /// <returns></returns>
        private float GetCurrentFeed()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Private Function which is deriving the current X in the cnc-controller
        /// </summary>
        /// <returns></returns>
        private float GetCurrentX()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Private Function which is deriving the Y in the cnc-controller
        /// </summary>
        /// <returns></returns>
        private float GetCurrentY()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Private Function which is deriving the current Z in the cnc-controller
        /// </summary>
        /// <returns></returns>
        public async Task<float> GetCurrentZ()
        {
            float r = 0;
            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetCurrentZMessage();
                Interface.SendMessage(message);
                CNCMessage output = Interface.ReceiveMessage(100);
                var tmp = Regex.Match(output.Message, @"(WPos:([-0-9]+.[0-9]+),([-0-9]+.[0-9]+),([-0-9]+.[0-9]+))").Groups[4].Value;
                r = float.Parse(tmp);
            });

            return r;
        }

        public async Task<CNCMessage> SendKillAlarm()
        {
            CNCMessage tmp = null;
            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetKillAlarmMessage();
                Interface.SendMessage(message);
                tmp = Interface.ReceiveMessage(100);
            });
            return tmp;
        }

        public async Task<CNCMessage> SendSoftReset()
        {
            CNCMessage tmp = null;
            await Task.Run(() =>
            {
                CNCMessage message = Protokoll.GetSoftResetMessage();
                Interface.SendMessage(message);
                tmp = Interface.ReceiveMessage(100);
            });

            return tmp;
        }

        /// <summary>
        /// Thish holds the current Interface. The Interface can be of different
        /// and this way it can connected via SerialPort, USB etc.
        /// </summary>
        public CNCInterface Interface { get; set; }

        /// <summary>
        /// Thish holds the current Protokoll for providing CNCMessages
        /// </summary>
        public CNCProtokoll Protokoll { get; set; }

        public MachineStates State
        {
            get => default;
            set
            {
            }
        }



        public CNC_Device(CNCInterface IFace, CNCProtokoll protokoll)
        {
            Interface = IFace;
            Protokoll = protokoll;
        }

        ~CNC_Device()
        {
        }
    }
}