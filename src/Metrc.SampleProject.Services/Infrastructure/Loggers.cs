using System;
using System.Linq;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Metrc.SampleProject.Services.Infrastructure
{
    public static class Loggers
    {
        private const String DefaultLogPattern = "[%d{yyyy-MM-dd HH:mm:ss.fff zzz}] [%-5p] [%t] %m%n";
        private static readonly PatternLayout DefaultPatternLayout;


        #region // Constructor
        // ==================================================

        static Loggers()
        {

            DefaultPatternLayout = new PatternLayout
            {
                ConversionPattern = DefaultLogPattern
            };
            DefaultPatternLayout.ActivateOptions();

            var tracer = new TraceAppender
            {
                Layout = DefaultPatternLayout
            };
            tracer.ActivateOptions();

            var hierarchy = (Hierarchy)LogManager.GetRepository(Assembly.GetEntryAssembly());
            hierarchy.Root.AddAppender(tracer);
#if (DEBUG)
            hierarchy.Root.Level = Level.All;
#else
            hierarchy.Root.Level = Level.Warn;
#endif
            hierarchy.Configured = true;
        }

        // ==================================================
        #endregion


        // For additional loggers/logs, please create a new Get<___>Logger() method
        #region // Logger Methods
        // ==================================================

        public static ILog GetExceptionLogger()
        {
            var result = GetLogger("ExceptionLogger", "Exception.log");
            return result;
        }

        public static ILog GetMigrationLogger()
        {
            var result = GetLogger("MigrationLogger", "Migration.log", Level.All);
            return result;
        }

        // ==================================================
        #endregion


        #region // Private Methods
        // ==================================================

        private static ILog GetLogger(String name, String logFileName, Level loggingLevel = null, PatternLayout patternLayout = null)
        {
            var result = LogManager.GetLogger(Assembly.GetEntryAssembly(), name);
            var logger = (Logger)result.Logger;

            if (loggingLevel != null && !ReferenceEquals(logger.Level, loggingLevel))
            {
                logger.Level = loggingLevel;
            }

            var appenders = logger.Appenders.OfType<RollingFileAppender>().ToArray();
            if (appenders.Length == 0)
            {
                var roller = new RollingFileAppender
                {
                    Name = name,
                    Layout = patternLayout ?? DefaultPatternLayout,
                    File = @"Logs\" + logFileName,
                    StaticLogFileName = true,
                    PreserveLogFileNameExtension = true,
                    AppendToFile = true,
                    RollingStyle = RollingFileAppender.RollingMode.Size,
                    MaxFileSize = 1048576, // 1 mb
                    MaxSizeRollBackups = 10,
                };
                roller.ActivateOptions();

                logger.AddAppender(roller);
            }
            else if (patternLayout != null)
            {
                foreach (var appender in appenders.Where(x => !ReferenceEquals(x.Layout, patternLayout)))
                {
                    appender.Layout = patternLayout;
                }
            }

            return result;
        }

        // ==================================================
        #endregion
    }
}
