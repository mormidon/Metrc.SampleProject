using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Metrc.SampleProject.Deployments.Infrastructure
{
    public class Log4netLoggerProvider : ILoggerProvider
    {
        private readonly FluentMigratorLoggerOptions _Options;

        public Log4netLoggerProvider(IOptions<FluentMigratorLoggerOptions> options)
        {
            _Options = options.Value;
        }

        public ILogger CreateLogger(String categoryName)
        {
            return new Log4netAdapter(_Options);
        }

        public void Dispose()
        {
        }
    }
}
