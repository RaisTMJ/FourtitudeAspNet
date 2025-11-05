using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;

namespace FourtitudeAspNet.Configuration
{
    public static class Log4NetConfiguration
    {
        public static void ConfigureLog4Net()
        {
            var logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            var hierarchy = (log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository();
            hierarchy.Root.Level = Level.Info;

            var rollingFileAppender = new RollingFileAppender
            {
                Name = "RollingFileAppender",
                File = Path.Combine(logsDirectory, "FourtitudeApi_"),
                AppendToFile = true,
                RollingStyle = RollingFileAppender.RollingMode.Date,
                DatePattern = "yyyyMMdd\".log\"",
                StaticLogFileName = false,
                Layout = new PatternLayout("%date{yyyy-MM-dd HH:mm:ss} [%thread] %-5level %logger - %message%newline")
            };

            rollingFileAppender.ActivateOptions();
            hierarchy.Root.AddAppender(rollingFileAppender);
            hierarchy.Configured = true;
        }
    }
}
