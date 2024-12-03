using Microsoft.Extensions.Configuration;

namespace PowerBank_AQA_UITestingCore.Extensions
{
    public sealed class ConfigurationExtensions
    {
        private static readonly Lazy<ConfigurationExtensions> Lazy
            = new(() => new ConfigurationExtensions());

        private ConfigurationExtensions()
        {

        }

        public static ConfigurationExtensions Instance
            => Lazy.Value;

        public IConfiguration Configuration { get; set; } = null;
    }
}
