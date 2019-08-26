using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PC
{
    // https://github.com/gnea/grbl/wiki/Grbl-v1.1-Commands

    public enum GRBLCommand
    {
        GRBL_ViewSettings,
        GRBL_WriteSetting,
        GRBL_ViewGCODEParameters,
        GRBL_ViewGCODEParserState,
        GRBL_ViewBuildInfo,
        GRBL_ViewStartUpBlocks,
        GRBL_SaveStartUpBlock,
        GRBL_CheckGCODEMode,
        GRBL_KillAlarmLock,
        GRBL_RunHomincCycle,
        GRBL_RunJoggingMogion,
        GRBL_ResetGRBLSettings,
        GRBL_ResetG54_G59Settings,
        GRBL_ResetEEPROM,
        GRBL_EnableSleepMode,
        RTC_SoftReset,
        RTC_StatusReportQuery,
        RTC_CycleStart_Resume,
        RTC_FeedHold,
        RTC_SafetyDoor,
        RTC_JogCancel,
        RTC_SetFeed100,
        RTC_IncreaseFeedBy10Percent,
        RTC_IncreaseFeedBy1Percent,
        RTC_DecreaseFeedBy10Percent,
        RTC_DecreaseFeedBy1Percent,
        RTC_SetSpindle100,
        RTC_IncreaseSpindleBy10Percent,
        RTC_IncreaseSpindleBy1Percent,
        RTC_DecreaseSpindleBy10Percent,
        RTC_DecreaseSpindleBy1Percent,
        RTC_SetRapid100,
        RTC_SetRapid50Percent,
        RTCSetRapid25Percent,
        RTC_ToggleSpindleStop,
        RTC_ToggleFloodCoolant,
        RTC_ToggleMistCoolant
    }
}