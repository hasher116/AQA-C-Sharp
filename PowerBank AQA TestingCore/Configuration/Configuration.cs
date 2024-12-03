using Microsoft.Extensions.Configuration;

namespace PowerBank_AQA_TestingCore.Configuration
{
    public static class Configuration
    {
        public static IConfiguration GetConfiguration()
        {
            return ConfigFile.CreateConfigureFile();
        }
    }
}
