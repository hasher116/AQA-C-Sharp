using PowerBank_AQA_UITestingCore.Infrastructures;

namespace PowerBank_AQA_UITestingCore.Models.Settings
{
    public class Settings : ISettings
    {
        private int? _timeout = Constants.DEFAULT_TIMEOUT;

        public BrowserType Browser { get; set; } = BrowserType.CHROME;

        public string BinaryLocation { get; set; }

        public string DriverLocation { get; set; }

        public List<string> Options { get; set; }

        public Dictionary<string, string> Capabilities { get; set; }

        public List<string> Extensions { get; set; }

        public Dictionary<string, string> UserProfilePreference { get; set; }

        public int Timeout
        {
            get => _timeout ?? Constants.DEFAULT_TIMEOUT;
            set => _timeout = value;
        }

        public bool IsRemote { get; set; } = false;

        public bool IsOptions() => Options != null;

        public bool IsExtensions()
        {
            if (Extensions == null)
            {
                return false;
            }

            return Extensions.Any();
        }

        public bool IsBinaryPath() => !string.IsNullOrWhiteSpace(BinaryLocation);

        public bool IsDriverPath() => !string.IsNullOrWhiteSpace(DriverLocation);

        public bool CheckCapability() => Capabilities != null;

        public bool CheckUserProfilePreference() => UserProfilePreference != null;
    }
}
