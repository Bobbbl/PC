using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PC
{
    public class GRBLCommandDictionary
    {
        private Dictionary<GRBLCommand, string> _CommandString = new Dictionary<GRBLCommand, string>()
        {
            { GRBLCommand.GRBL_ViewSettings, "$$"},
            {GRBLCommand.GRBL_WriteSetting, "&@@=@@@"},
            {GRBLCommand.GRBL_ViewGCODEParameters, "$#"},
            {GRBLCommand.GRBL_ViewGCODEParserState, "$G"},
            {GRBLCommand.GRBL_ViewBuildInfo, "$I"},
            {GRBLCommand.GRBL_ViewStartUpBlocks, "$N"},
            {GRBLCommand.GRBL_SaveStartUpBlock, "$N@@=@@@"},
            {GRBLCommand.GRBL_CheckGCODEMode, "$C"},
            {GRBLCommand.GRBL_KillAlarmLock, "$X"},
            {GRBLCommand.GRBL_RunHomincCycle, "$H"},
            {GRBLCommand.GRBL_RunJoggingMogion, "$J="},
            {GRBLCommand.GRBL_ResetGRBLSettings, "$RST=$"},
            {GRBLCommand.GRBL_ResetG54_G59Settings, "$RST=#"},
            {GRBLCommand.GRBL_ResetEEPROM, "$RST=*"},
            {GRBLCommand.GRBL_EnableSleepMode, "$SLP"},
            {GRBLCommand.RTC_SoftReset, Convert.ToString(0x18)},
            {GRBLCommand.RTC_StatusReportQuery, "?"},
            {GRBLCommand.RTC_CycleStart_Resume, "~"},
            {GRBLCommand.RTC_FeedHold, "!"},
            {GRBLCommand.RTC_SafetyDoor, Convert.ToString(0x84)},
            {GRBLCommand.RTC_JogCancel, Convert.ToString(0x85)},
            {GRBLCommand.RTC_SetFeed100, Convert.ToString(0x90)},
            {GRBLCommand.RTC_IncreaseFeedBy10Percent, Convert.ToString(0x91)},
            {GRBLCommand.RTC_IncreaseFeedBy1Percent, Convert.ToString(0x93)},
            {GRBLCommand.RTC_DecreaseFeedBy10Percent, Convert.ToString(0x92)},
            {GRBLCommand.RTC_DecreaseFeedBy1Percent, Convert.ToString(0x94)},
            {GRBLCommand.RTC_SetSpindle100, Convert.ToString(0x99)},
            {GRBLCommand.RTC_IncreaseSpindleBy10Percent, Convert.ToString(0x9A)},
            {GRBLCommand.RTC_IncreaseSpindleBy1Percent, Convert.ToString(0x9C)},
            {GRBLCommand.RTC_DecreaseSpindleBy10Percent, Convert.ToString(0x9B)},
            {GRBLCommand.RTC_DecreaseSpindleBy1Percent, Convert.ToString(0x9D)},
            {GRBLCommand.RTC_SetRapid100, Convert.ToString(0x95)},
            {GRBLCommand.RTC_SetRapid50Percent, Convert.ToString(0x96)},
            {GRBLCommand.RTCSetRapid25Percent, Convert.ToString(0x97)},
            {GRBLCommand.RTC_ToggleSpindleStop, Convert.ToString(0x9E)},
            {GRBLCommand.RTC_ToggleFloodCoolant, Convert.ToString(0xA0)},
            {GRBLCommand.RTC_ToggleMistCoolant, Convert.ToString(0xA1)},
            {GRBLCommand.G_SetUnitToMillimeter_G21, "G21" },
            {GRBLCommand.G_RelativeMotion_G91, "G91" }

        };

        public Dictionary<GRBLCommand, string> CommandString
        {
            get { return CommandString; }
            set { CommandString = value; }
        }


        public string GetCommand(GRBLCommand command, string bezeichner = "", string wert = "")
        {
            string r = string.Empty;

            r = _CommandString[command];

            if(r.Contains("@@"))
            {
                r.Replace("@@", bezeichner);
            }

            if(r.Contains("@@@"))
            {
                r.Replace("@@@", wert);
            }

            return r;
        }
    }
}