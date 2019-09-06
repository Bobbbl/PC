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
            {GRBLCommand.GRBL_WriteSetting, "&@="},
            {GRBLCommand.GRBL_ViewGCODEParameters, "$#"},
            {GRBLCommand.GRBL_ViewGCODEParserState, "$G"},
            {GRBLCommand.GRBL_ViewBuildInfo, "$I"},
            {GRBLCommand.GRBL_ViewStartUpBlocks, "$N"},
            {GRBLCommand.GRBL_SaveStartUpBlock, "$N@@@="},
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
            {GRBLCommand.RTC_ToggleMistCoolant, Convert.ToString(0xA1)}

        };

        public Dictionary<GRBLCommand, string> CommandString
        {
            get { return CommandString; }
            set { CommandString = value; }
        }


        public string GetCommand(GRBLCommand command)
        {

            string r = string.Empty;

            switch (command)
            {
                case GRBLCommand.GRBL_ViewSettings:
                    r = "$$";
                    break;
                case GRBLCommand.GRBL_WriteSetting:
                    // Hier muss dann @ in der entsprechenden Funktion ersetzt werden durch die gewünschte Option
                    r = "&@=";
                    break;
                case GRBLCommand.GRBL_ViewGCODEParameters:
                    r = "$#";
                    break;
                case GRBLCommand.GRBL_ViewGCODEParserState:
                    r = "$G";
                    break;
                case GRBLCommand.GRBL_ViewBuildInfo:
                    r = "$I";
                    break;
                case GRBLCommand.GRBL_ViewStartUpBlocks:
                    r = "$N";
                    break;
                case GRBLCommand.GRBL_SaveStartUpBlock:
                    // Hier muss dann @ in der entsprechenden Funktion ersetzt werden durch die gewünschte Line
                    r = "$N@=";
                    break;
                case GRBLCommand.GRBL_CheckGCODEMode:
                    r = "$C";
                    break;
                case GRBLCommand.GRBL_KillAlarmLock:
                    r = "$X";
                    break;
                case GRBLCommand.GRBL_RunHomincCycle:
                    r = "$H";
                    break;
                case GRBLCommand.GRBL_RunJoggingMogion:
                    r = "$J=";
                    break;
                case GRBLCommand.GRBL_ResetGRBLSettings:
                    r = "$RST=$";
                    break;
                case GRBLCommand.GRBL_ResetG54_G59Settings:
                    r = "$RST=#";
                    break;
                case GRBLCommand.GRBL_ResetEEPROM:
                    r = "$RST=*";
                    break;
                case GRBLCommand.GRBL_EnableSleepMode:
                    r = "$SLP";
                    break;
                case GRBLCommand.RTC_SoftReset:
                    r = Convert.ToString(0x18);
                    break;
                case GRBLCommand.RTC_StatusReportQuery:
                    r = "?";
                    break;
                case GRBLCommand.RTC_CycleStart_Resume:
                    r = "~";
                    break;
                case GRBLCommand.RTC_FeedHold:
                    r = "!";
                    break;
                case GRBLCommand.RTC_SafetyDoor:
                    r = Convert.ToString(0x84);
                    break;
                case GRBLCommand.RTC_JogCancel:
                    r = Convert.ToString(0x85);
                    break;
                case GRBLCommand.RTC_SetFeed100:
                    r = Convert.ToString(0x90);
                    break;
                case GRBLCommand.RTC_IncreaseFeedBy10Percent:
                    r = Convert.ToString(0x91);
                    break;
                case GRBLCommand.RTC_IncreaseFeedBy1Percent:
                    r = Convert.ToString(0x93);
                    break;
                case GRBLCommand.RTC_DecreaseFeedBy10Percent:
                    r = Convert.ToString(0x92);
                    break;
                case GRBLCommand.RTC_DecreaseFeedBy1Percent:
                    r = Convert.ToString(0x94);
                    break;
                case GRBLCommand.RTC_SetRapid100:
                    r = Convert.ToString(0x95);
                    break;
                case GRBLCommand.RTC_SetRapid50Percent:
                    r = Convert.ToString(0x96);
                    break;
                case GRBLCommand.RTCSetRapid25Percent:
                    r = Convert.ToString(0x97);
                    break;
                case GRBLCommand.RTC_SetSpindle100:
                    r = Convert.ToString(0x99);
                    break;
                case GRBLCommand.RTC_IncreaseSpindleBy10Percent:
                    r = Convert.ToString(0x9A);
                    break;
                case GRBLCommand.RTC_IncreaseSpindleBy1Percent:
                    r = Convert.ToString(0x9C);
                    break;
                case GRBLCommand.RTC_DecreaseSpindleBy10Percent:
                    r = Convert.ToString(0x9B);
                    break;
                case GRBLCommand.RTC_DecreaseSpindleBy1Percent:
                    r = Convert.ToString(0x9D);
                    break;
                case GRBLCommand.RTC_ToggleSpindleStop:
                    r = Convert.ToString(0x9E);
                    break;
                case GRBLCommand.RTC_ToggleFloodCoolant:
                    r = Convert.ToString(0xA0);
                    break;
                case GRBLCommand.RTC_ToggleMistCoolant:
                    r = Convert.ToString(0xA1);
                    break;

                default:
                    break;
            }

            return r;
        }
    }
}