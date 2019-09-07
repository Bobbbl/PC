using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PC
{
    public class GRBLProtokoll : CNCProtokoll
    {

        public GRBLCommandDictionary CommandDict { get; set; } = new GRBLCommandDictionary();

        /// <summary>
        /// Gets a GRBL Message for Feed
        /// </summary>
        /// <returns>CNCMessage</returns>
        public override CNCMessage GetCurrentFeedMessage()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a GRBL Message for X
        /// </summary>
        /// <returns>CNCMessage</returns>
        public override CNCMessage GetCurrentXMessage()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a GRBL Message for XYZ Coordinates
        /// </summary>
        /// <returns>CNCMessage</returns>
        public override CNCMessage GetCurrentXYZMessage()
        {
            CNCMessage rmessage = new CNCMessage();
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.RTC_StatusReportQuery));

            return rmessage;
        }

        /// <summary>
        /// Gets a GRBL Message for Y 
        /// </summary>
        /// <returns>CNCMessage</returns>
        public override CNCMessage GetCurrentYMessage()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a GRBL Message for Z
        /// </summary>
        /// <returns>CNCMessage</returns>
        public override CNCMessage GetCurrentZMessage()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a GRBL Message for moving the tool by ... Millimeters in X direction
        /// </summary>
        /// <returns></returns>
        public override CNCMessage GetMoveByXMessage()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a GRBL Message for moving the bool by ... Millimeters in Y direction
        /// </summary>
        /// <returns></returns>
        public override CNCMessage GetMoveByYMessage()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a GRBL Message for moving the tool by ... Millimeters in Z direction
        /// </summary>
        /// <returns></returns>
        public override CNCMessage GetMoveByZMessage()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a GRBL Message for setting the feed of the CNC Controller
        /// </summary>
        /// <returns></returns>
        public override CNCMessage GetSetFeedMessage()
        {
            throw new NotImplementedException();
        }

        public void GetStatusReportMessage()
        {
            throw new System.NotImplementedException();
        }
    }
}