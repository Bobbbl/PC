using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
            if(PropertyChanged != null)
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
        private float GetCurrentZ()
        {
            throw new NotImplementedException();
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
    }
}