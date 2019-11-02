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
            {GRBLCommand.GRBL_WriteSetting, "$@@=%%% "},
            {GRBLCommand.GRBL_WriteValue, "@@%%% "},
            {GRBLCommand.GRBL_ViewGCODEParameters, "$#"},
            {GRBLCommand.GRBL_ViewGCODEParserState, "$G"},
            {GRBLCommand.GRBL_ViewBuildInfo, "$I"},
            {GRBLCommand.GRBL_ViewStartUpBlocks, "$N"},
            {GRBLCommand.GRBL_SaveStartUpBlock, "$N@@=%%% "},
            {GRBLCommand.GRBL_CheckGCODEMode, "$C"},
            {GRBLCommand.GRBL_KillAlarmLock, "$X"},
            {GRBLCommand.GRBL_RunHomincCycle, "$H"},
            {GRBLCommand.GRBL_RunJoggingMogion, "$J="},
            {GRBLCommand.GRBL_ResetGRBLSettings, "$RST=$"},
            {GRBLCommand.GRBL_ResetG54_G59Settings, "$RST=#"},
            {GRBLCommand.GRBL_ResetEEPROM, "$RST=*"},
            {GRBLCommand.GRBL_EnableSleepMode, "$SLP"},
            {GRBLCommand.RTC_SoftReset, Convert.ToString((char)0x18)},
            {GRBLCommand.RTC_StatusReportQuery, "?"},
            {GRBLCommand.RTC_CycleStart_Resume, "~"},
            {GRBLCommand.RTC_FeedHold, "!"},
            {GRBLCommand.RTC_SafetyDoor, Convert.ToString((char)0x84)},
            {GRBLCommand.RTC_JogCancel, Convert.ToString((char)0x85)},
            {GRBLCommand.RTC_SetFeed100, Convert.ToString((char)0x90)},
            {GRBLCommand.RTC_IncreaseFeedBy10Percent, Convert.ToString((char)0x91)},
            {GRBLCommand.RTC_IncreaseFeedBy1Percent, Convert.ToString((char)0x93)},
            {GRBLCommand.RTC_DecreaseFeedBy10Percent, Convert.ToString((char)0x92)},
            {GRBLCommand.RTC_DecreaseFeedBy1Percent, Convert.ToString((char)0x94)},
            {GRBLCommand.RTC_SetSpindle100, Convert.ToString((char)0x99)},
            {GRBLCommand.RTC_IncreaseSpindleBy10Percent, Convert.ToString((char)0x9A)},
            {GRBLCommand.RTC_IncreaseSpindleBy1Percent, Convert.ToString((char)0x9C)},
            {GRBLCommand.RTC_DecreaseSpindleBy10Percent, Convert.ToString((char)0x9B)},
            {GRBLCommand.RTC_DecreaseSpindleBy1Percent, Convert.ToString((char)0x9D)},
            {GRBLCommand.RTC_SetRapid100, Convert.ToString((char)0x95)},
            {GRBLCommand.RTC_SetRapid50Percent, Convert.ToString((char)0x96)},
            {GRBLCommand.RTCSetRapid25Percent, Convert.ToString((char)0x97)},
            {GRBLCommand.RTC_ToggleSpindleStop, Convert.ToString((char)0x9E)},
            {GRBLCommand.RTC_ToggleFloodCoolant, Convert.ToString((char)0xA0)},
            {GRBLCommand.RTC_ToggleMistCoolant, Convert.ToString((char)0xA1)},
            {GRBLCommand.G_SetUnitToMillimeter_G21, "G21 " },
            {GRBLCommand.G_RelativeMotion_G91, "G91 " },
            {GRBLCommand.G_SetOffset_G10, "G10 " },
            {GRBLCommand.L1, "L1 " },
            {GRBLCommand.L2, "L2 " },
            {GRBLCommand.L20, "L20 " },
            {GRBLCommand.P_Tool1, "P1 " },
            {GRBLCommand.P_Tool2, "P2 " },
            {GRBLCommand.P_Tool3, "P3 " },
            {GRBLCommand.P_Tool4, "P4 " },
            {GRBLCommand.P_Tool5, "P5 " },
            {GRBLCommand.P_Tool6, "P6 " },
            {GRBLCommand.P_Tool7, "P7 " },
            {GRBLCommand.P_Tool8, "P8 " },
            {GRBLCommand.P_Tool9, "P9 " },
            {GRBLCommand.P_Tool10, "P10 " }

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
                r = r.Replace("@@", bezeichner);
            }

            if(r.Contains("%%%"))
            {
                r = r.Replace("%%%", wert);
            }

            return r;
        }
    }
}