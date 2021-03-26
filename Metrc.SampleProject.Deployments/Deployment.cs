using System;
using Metrc.SampleProject.Deployments.Infrastructure;

namespace Metrc.SampleProject.Deployments
{
    public static class Deployment
    {
        public static void CleanDatabase(String connectionString)
        {
            new CleanRelationalDb(connectionString).Execute();
        }

        public static void MigrateDatabase(String connectionString)
        {
            new MigrateRelationalDb(connectionString, typeof(MigrateRelationalDb).Assembly).ExecuteSqlServer();
        }
    }
}
