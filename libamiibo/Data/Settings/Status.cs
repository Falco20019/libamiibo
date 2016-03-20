using System;

namespace LibAmiibo.Data.Settings
{
    [Flags]
    public enum Status
    {
        SettingsInitialized = 1 << 4,
        AppDataInitialized = 1 << 5
    }
}