using PowerBank_AQA_TestingCore.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PowerBank_AQA_TestingCore.Configuration
{
    public static class ConfigFile
    {
        public static IConfiguration CreateConfigureFile()
        {
            string? fileName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (fileName == null)
            {
                Log.Logger().LogInformation("Запуск тестов происходит в окружении по умолчанию");
                fileName = "appsettings46env.test.json";
            }
            else
            {
                Log.Logger().LogInformation($"Запуск тестов происходит в окружении {fileName}");
            }
            try
            {
                if (Environment.GetEnvironmentVariable("HOSTNAME") != null)
                {
                    Log.Logger().LogInformation("Running inside a Docker container.");
                    var builder = new ConfigurationBuilder()

                 .SetBasePath(Directory.GetCurrentDirectory() + "/Settings/")
                 .AddJsonFile(fileName, optional: false, reloadOnChange: true);
                    Log.Logger().LogInformation($"Список переменных тестовой среды успешно сформирован");
                    IConfiguration config = builder.Build();

                    return config;
                }
                else
                {
                    Log.Logger().LogInformation("Not running inside a Docker container.");
                    var builder = new ConfigurationBuilder()

                 .SetBasePath(Directory.GetCurrentDirectory() + "\\Settings\\")
                 .AddJsonFile(fileName, optional: false, reloadOnChange: true);
                    Log.Logger().LogInformation($"Список переменных тестовой среды успешно сформирован");
                    IConfiguration config = builder.Build();

                    return config;
                }

            }
            catch (Exception ex)
            {
                Log.Logger().LogError($"Не удалось сформировать список переменных для тестовой среды", ex.Message);
                throw;
            }
        }
    }
}
