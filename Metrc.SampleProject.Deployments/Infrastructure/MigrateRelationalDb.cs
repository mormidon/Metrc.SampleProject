using System;
using System.Reflection;
using FluentMigrator.Runner;
using Metrc.SampleProject.Deployments.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Metrc.SampleProject.Deployments.Infrastructure
{
    public sealed class MigrateRelationalDb
    {
        private readonly String _ConnectionString;
        private readonly Assembly[] _MigrationAssemblies;

        public MigrateRelationalDb(String connectionString, params Assembly[] migrationAssemblies)
        {
            _ConnectionString = connectionString;
            _MigrationAssemblies = migrationAssemblies;
        }

        public void ExecuteSqlServer()
        {
            var serviceProvider = CreateServices(_ConnectionString, _MigrationAssemblies);

            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
        }

        private static IServiceProvider CreateServices(String connectionString, params Assembly[] migrations)
        {
            var result = new ServiceCollection()
                .AddLogging(x => x.AddLog4netLogger())
                .AddFluentMigratorCore()
                .ConfigureRunner(x => x
                    .AddSqlServer()
                    .WithGlobalConnectionString(connectionString)
                    .WithGlobalCommandTimeout(TimeSpan.Zero)
                    .ConfigureGlobalProcessorOptions(po => po.Timeout = TimeSpan.Zero)
                    .ScanIn(migrations)
                    .For.Migrations()
                    .For.VersionTableMetaData())
                .Configure<FluentMigratorLoggerOptions>(cfg =>
                {
                    cfg.ShowElapsedTime = true;
                    cfg.ShowSql = true;
                })
                .BuildServiceProvider(false);
            return result;
        }
    }
}
