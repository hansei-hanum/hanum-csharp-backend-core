using Microsoft.Extensions.Logging.Console;

using NReco.Logging.File;

namespace Hanum.Core.Helpers;

public static class HanumLoggingServiceCollectionExtensions {
    public static IServiceCollection AddHanumLogging(this IServiceCollection services) =>
        services.AddLogging(logging => {
            logging.AddSimpleConsole(options => {
                options.SingleLine = true;
                options.TimestampFormat = "HH:mm:ss ";
                options.ColorBehavior = LoggerColorBehavior.Enabled;
            });

            logging.AddFile("logs/app_{0:yyyy}-{0:MM}-{0:dd}.log", options => {
                options.FormatLogFileName = name => string.Format(name, DateTime.UtcNow); ;
            });
        });
}
