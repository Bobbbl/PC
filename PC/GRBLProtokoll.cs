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
        /// Gets a GRBL Message for Feed. 
        /// This is the message who will returned
        /// [GC:G0 G54 G17 G21 G90 G94 M5 M9 T0 F0 S0]        /// </summary>
        /// <returns>CNCMessage</returns>
        public override CNCMessage GetCurrentFeedMessage()
        {
            CNCMessage rmessage = new CNCMessage();
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_ViewGCODEParserState));

            return rmessage;
        }

        /// <summary>
        /// Get a GRBL Message for X
        /// </summary>
        /// <returns>CNCMessage</returns>
        public override CNCMessage GetCurrentXMessage()
        {
            CNCMessage rmessage = new CNCMessage();
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.RTC_StatusReportQuery));

            return rmessage;
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
            CNCMessage rmessage = new CNCMessage();
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.RTC_StatusReportQuery));

            return rmessage;
        }

        /// <summary>
        /// Gets a GRBL Message for Z
        /// </summary>
        /// <returns>CNCMessage</returns>
        public override CNCMessage GetCurrentZMessage()
        {
            CNCMessage rmessage = new CNCMessage();
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.RTC_StatusReportQuery));

            return rmessage;
        }

        /// <summary>
        /// Gets a GRBL Message for moving the tool by ... Millimeters in X direction
        /// </summary>
        /// <returns></returns>
        public override CNCMessage GetMoveByXMessage(double XMillimieter, double Feed)
        {
            CNCMessage rmessage = new CNCMessage();
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_WriteSetting, "X", XMillimieter.ToString()));
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_WriteSetting, "F", Feed.ToString()));

            return rmessage;
        }

        /// <summary>
        /// Gets a GRBL Message for moving the bool by ... Millimeters in Y direction
        /// </summary>
        /// <returns></returns>
        public override CNCMessage GetMoveByYMessage(double YMillimieter, double Feed)
        {
            CNCMessage rmessage = new CNCMessage();
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_WriteSetting, "Y", YMillimieter.ToString()));
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_WriteSetting, "F", Feed.ToString()));

            return rmessage;
        }

        /// <summary>
        /// Gets a GRBL Message for moving the tool by ... Millimeters in Z direction
        /// </summary>
        /// <returns></returns>
        public override CNCMessage GetMoveByZMessage(double ZMillimieter, double Feed)
        {
            CNCMessage rmessage = new CNCMessage();
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_WriteSetting, "Z", ZMillimieter.ToString()));
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_WriteSetting, "F", Feed.ToString()));

            return rmessage;
        }

        /// <summary>
        /// Gets a GRBL Message for setting the feed of the CNC Controller
        /// </summary>
        /// <returns></returns>
        public override CNCMessage GetSetFeedMessage(double Feed)
        {
            CNCMessage rmessage = new CNCMessage();
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_WriteSetting, "F", Feed.ToString()));

            return rmessage;
        }

        public CNCMessage GetJogByXMessage(double XMillimieter, double Feed)
        {
            CNCMessage rmessage = new CNCMessage();
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_RunJoggingMogion));
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.G_SetUnitToMillimeter_G21));
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.G_RelativeMotion_G91));
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_WriteSetting, "X", XMillimieter.ToString()));
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_WriteSetting, "F", Feed.ToString()));

            return rmessage;
        }

        public CNCMessage GetJogByYMessage(double YMillimieter, double Feed)
        {
            CNCMessage rmessage = new CNCMessage();
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_RunJoggingMogion));
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.G_SetUnitToMillimeter_G21));
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.G_RelativeMotion_G91));
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_WriteSetting, "Y", YMillimieter.ToString()));
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_WriteSetting, "F", Feed.ToString()));

            return rmessage;
        }

        public CNCMessage GetJogByZMessage(double ZMillimieter, double Feed)
        {
            CNCMessage rmessage = new CNCMessage();
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_RunJoggingMogion));
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.G_SetUnitToMillimeter_G21));
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.G_RelativeMotion_G91));
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_WriteSetting, "Z", ZMillimieter.ToString()));
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.GRBL_WriteSetting, "F", Feed.ToString()));

            return rmessage;
        }

        public CNCMessage GetStatusReportMessage()
        {
            CNCMessage rmessage = new CNCMessage();
            rmessage.AppendCommand(CommandDict.GetCommand(GRBLCommand.RTC_StatusReportQuery));

            return rmessage;
        }

    }
}