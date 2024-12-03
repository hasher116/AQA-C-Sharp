using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PowerBank_AQA_UITestingCore.Infrastructures;
using PowerBank_AQA_UITestingCore.Models.Settings;

namespace PowerBank_AQA_UITestingCore.Helpers
{
    public static class ConfigOptionsFactory
    {
        //TODO: проверка на null
        public static IOptions<Settings> Create(IConfiguration configuration)
        {
            var section = configuration.GetSection(Constants.CONFIG_BLOCK).GetSection(Constants.SETTINGS_BLOCK);

            var settings = section.Get<Settings>();
            return Options.Create(settings);
        }
    }
}
