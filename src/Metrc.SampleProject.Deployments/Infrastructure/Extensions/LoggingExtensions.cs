using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Metrc.SampleProject.Deployments.Infrastructure.Extensions
{
    public static class LoggingExtensions
    {
        public static ILoggingBuilder AddLog4netLogger(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.Services.AddSingleton<ILoggerProvider, Log4netLoggerProvider>();
            return loggingBuilder;
        }
    }
}
