using System;

namespace PowerBank_AQA_UITestingCore.Models.Settings
{
    public interface ISettings
    {
        bool IsBinaryPath();

        bool IsOptions();

        bool CheckCapability();

        bool CheckUserProfilePreference();
    }
}
