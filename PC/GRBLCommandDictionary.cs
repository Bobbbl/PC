using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PC
{
    public class GRBLCommandDictionary
    {
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