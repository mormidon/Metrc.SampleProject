using System;
using System.Data.Common;

namespace Metrc.SampleProject.Services.Infrastructure
{
    public class RepositoryConnectionFactory
    {
        public RepositoryConnectionFactory(String applicationName, String connectionString)
            : this(applicationName, connectionString, DbProviderFactories.GetFactory("System.Data.SqlClient"))
        {
        }

        public RepositoryConnectionFactory(String applicationName, String connectionString, DbProviderFactory dbProviderFactory)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder(false)
            {
                ConnectionString = connectionString,
            };
            connectionStringBuilder.Add("Application Name", applicationName);

            ConnectionString = connectionStringBuilder.ToString();
            DbProviderFactory = dbProviderFactory;
        }

        public String ConnectionString { get; set; }
        public DbProviderFactory DbProviderFactory { get; set; }

        public DbConnection CreateDbConnection()
        {
            if (ConnectionString == null)
            {
                throw new InvalidOperationException("ConnectionString must be set");
            }

            var connection = DbProviderFactory.CreateConnection();
            if (connection == null)
            {
                throw new InvalidOperationException("DbProviderFactory was not able to create a DbConnection");
            }

            var result = new RepositoryDbConnection(connection)
            {
                ConnectionString = ConnectionString
            };
            return result;
        }

        public DbConnection OpenDbConnection()
        {
            var connection = CreateDbConnection();
            connection.Open();
            return connection;
        }
    }
}
