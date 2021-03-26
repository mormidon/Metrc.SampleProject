using System;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Logging;
using log4net;
using Metrc.SampleProject.Services.Infrastructure;

namespace Metrc.SampleProject.Deployments.Infrastructure
{
    public class Log4netAdapter : FluentMigratorLogger
    {
        ILog _Logger;

        public Log4netAdapter(FluentMigratorLoggerOptions options) : base(options)
        {
            _Logger = Loggers.GetMigrationLogger();
        }

        protected override void WriteElapsedTime(TimeSpan timeSpan)
        {
            _Logger.Info($"Migration elapsed time: {timeSpan.TotalSeconds:#,##0.###}");
        }

        protected override void WriteEmphasize(String message)
        {
            _Logger.Info(message);
        }

        protected override void WriteEmptySql()
        {
            _Logger.Warn("Writing empty SQL");
        }

        protected override void WriteError(String message)
        {
            _Logger.Error($"Error performing migration: {message}");
        }

        protected override void WriteError(Exception exception)
        {
            _Logger.Error($"Error performing migration: {exception}");
        }

        protected override void WriteHeading(String message)
        {
            _Logger.Info(message);
        }

        protected override void WriteSay(String message)
        {
            _Logger.Info(message);
        }

        protected override void WriteSql(String sql)
        {
            _Logger.Info($"SQL statements performed: {sql}");
        }
    }
}
