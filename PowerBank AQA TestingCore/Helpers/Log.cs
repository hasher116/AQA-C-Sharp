using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace PowerBank_AQA_TestingCore.Helpers
{
    public static class Log
    {
        private static ILoggerFactory _factory;

        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (_factory != null)
                {
                    return _factory;
                }

                _factory = new LoggerFactory();
                ConfigureLogger(_factory);
                return _factory;
            }

            set => _factory = value;
        }

        public static ILogger Logger() => LoggerFactory.CreateLogger("Default");

        private static void ConfigureLogger(ILoggerFactory factory)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            factory.AddSerilog(logger);
        }
    }
}
