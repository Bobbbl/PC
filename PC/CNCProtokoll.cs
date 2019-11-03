using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PC
{
    /// <summary>
    /// This class acts as the template for the chosen Message Generators
    /// </summary>
    public abstract class CNCProtokoll
    {
        /// <summary>
        /// Provides the template for the GetCurrentFeedMessage
        /// </summary>
        /// <returns>CNCMessage</returns>
        public abstract CNCMessage GetCurrentFeedMessage();

        /// <summary>
        /// Provides the template for the GetCurrentXMessage - Function
        /// </summary>
        /// <returns>CNCMessage</returns>
        public abstract CNCMessage GetCurrentXMessage();

        /// <summary>
        /// Provides the template for the GetCurrentYMessage - Function
        /// </summary>
        /// <returns>CNCMessage</returns>
        public abstract CNCMessage GetCurrentYMessage();

        /// <summary>
        /// Provides the template for the GetCurrentYMessage - Function
        /// </summary>
        /// <returns></returns>
        public abstract CNCMessage GetCurrentZMessage();

        /// <summary>
        /// Provides the template for the GetCurrentZMessage
        /// </summary>
        /// <returns></returns>
        public abstract CNCMessage GetCurrentXYZMessage();

        /// <summary>
        /// Provides the template for the GetMoveByXMessage
        /// </summary>
        /// <returns></returns>
        public abstract CNCMessage GetMoveByXMessage(double XMillimieter, double Feed);

        /// <summary>
        /// Provides the template for the GetMoveByYMessage
        /// </summary>
        /// <returns></returns>
        public abstract CNCMessage GetMoveByYMessage(double YMillimieter, double Feed);

        /// <summary>
        /// Provides the template for the GetMoveByZMessage
        /// </summary>
        /// <returns></returns>
        public abstract CNCMessage GetMoveByZMessage(double ZMillimieter, double Feed);

        /// <summary>
        /// Provides the template for the GetMoveByXYZMessage
        /// </summary>
        /// <returns></returns>
        public abstract CNCMessage GetMoveByXYZMessage(double XMillimieter, double YMillimeter, double ZMillimeter, double Feed);

        /// <summary>
        /// Provides the template for the GetSetFeedMessage
        /// </summary>
        /// <returns></returns>
        public abstract CNCMessage GetSetFeedMessage(double Feed);

        public abstract CNCMessage GetRelativeJogByXMessage(double XMillimieter, double Feed);
        public abstract CNCMessage GetRelativeJogByYMessage(double YMillimieter, double Feed);
        public abstract CNCMessage GetRelativeJogByZMessage(double ZMillimeter, double Feed);
        public abstract CNCMessage GetRelativeJogByXYZMessage(double XMillimieter, double YMillimeter, double ZMillimeter, double Feed);
        public abstract CNCMessage GetJogByXMessage(double XMillimieter, double Feed);
        public abstract CNCMessage GetJogByYMessage(double YMillimieter, double Feed);
        public abstract CNCMessage GetJogByZMessage(double ZMillimieter, double Feed);
        public abstract CNCMessage GetJogByXYZMessage(double XMillimeter, double YMillimeter, double ZMillimeter, double Feed);
        public abstract CNCMessage GetStatusReportMessage();
        public abstract CNCMessage GetSoftResetMessage();
        public abstract CNCMessage GetKillAlarmMessage();
        public abstract CNCMessage GetSetZeroMessage();
        public abstract CNCMessage GetHomingMessage();
        public abstract CNCMessage GetSpindelSetRPMMessage(double RPM, string Direction);

    }
}