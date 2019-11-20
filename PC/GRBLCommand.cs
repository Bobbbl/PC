namespace PC
{
    // https://github.com/gnea/grbl/wiki/Grbl-v1.1-Commands

    public enum GRBLCommand
    {
        GRBL_ViewSettings,
        GRBL_WriteSetting,
        GRBL_WriteValue,
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
        RTC_ToggleMistCoolant,
        RTC_SetSpindleSpeedClockWise,
        RTC_SetSpindleSpeedCounterClockWise,
        RTC_StopSpindle,


        G_SetUnitToMillimeter_G21,
        G_RelativeMotion_G91,
        G_SetOffset_G10,

        L1,
        L2,
        L20,

        P_Tool1,
        P_Tool2,
        P_Tool3,
        P_Tool4,
        P_Tool5,
        P_Tool6,
        P_Tool7,
        P_Tool8,
        P_Tool9,
        P_Tool10


    }
}