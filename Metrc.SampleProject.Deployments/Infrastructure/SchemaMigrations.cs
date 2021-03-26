using System;
using FluentMigrator.Runner.VersionTableInfo;

namespace Metrc.SampleProject.Deployments.Infrastructure
{
    [VersionTableMetaData]
    public sealed class SchemaMigrations : IVersionTableMetaData
    {
        public Object ApplicationContext { get; set; }
        public String SchemaName { get { return String.Empty; } }
        public String TableName { get { return "SchemaMigrations"; } }
        public String ColumnName { get { return "Version"; } }
        public String UniqueIndexName { get { return "UC_SchemaMigrations"; } }
        public String AppliedOnColumnName { get { return "AppliedOn"; } }
        public String DescriptionColumnName { get { return "Description"; } }
        public Boolean OwnsSchema { get { return true; } }
    }
}
