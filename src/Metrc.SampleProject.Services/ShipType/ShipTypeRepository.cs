using System;
using System.Linq;
using Dapper;
using Metrc.SampleProject.Contracts.ShipType;
using Metrc.SampleProject.Services.Infrastructure;
using Metrc.SampleProject.Services.Infrastructure.Dapper;

namespace Metrc.SampleProject.Services.ShipType
{
    public class ShipTypeRepository
    {
        private RepositoryConnectionFactory DbFactory;

        public ShipTypeRepository(RepositoryConnectionFactory dbFactory)
        {
            DbFactory = dbFactory;
        }

        public Paged<ShipTypeDocument> GetShipType(Boolean? archivedState = false)
        {
            var result = QueryShipType(builder =>
            {
                if (archivedState.HasValue)
                {
                    switch (archivedState.Value)
                    {
                        case true:
                            builder.Where("IsArchived = @isArchived", new { isArchived = true });
                            break;
                        case false:
                            builder.Where("IsArchived = @isArchived", new { isArchived = false });
                            break;
                    }
                }
            });

            return result;
        }

        public ShipTypeDocument GetShipTypeById(Int64 id)
        {
            var result = QueryShipType(builder => { builder.Where("Id = @id", new { id }); });
            return result.Data.FirstOrDefault();
        }

        public void Create(String name, Double topSpeed)
        {
            using (var db = DbFactory.OpenDbConnection())
            {
                db.Execute(@"
INSERT INTO dbo.ShipType(
    Name,
    TopSpeed,
    IsArchived
) VALUES (
    @name,
    @topSpeed,
    @isArchived
);",
                    new
                    {
                        name,
                        topSpeed,
                        isArchived = false
                    });

            }
        }

        public void Update(Int64 id, String name, Double topSpeed)
        {
            using (var db = DbFactory.OpenDbConnection())
            {
                db.Execute(@"
UPDATE dbo.ShipType
SET Name = @name,
    TopSpeed = @topSpeed
WHERE Id = @id;",
                    new
                    {
                        id,
                        name,
                        topSpeed
                    });
            }
        }

        public void Archive(Int64 id)
        {
            using (var db = DbFactory.OpenDbConnection())
            {
                db.Execute("UPDATE dbo.ShipType SET IsArchived = @isArchived WHERE Id = @id", new { id, isArchived = true });
            }
        }

        public Paged<ShipTypeDocument> QueryShipType(Action<SqlBuilder> query = null)
        {
            var builder = new SqlBuilder();
            var selector = builder.AddTemplate(@"
SELECT
    Id,
    Name,
    TopSpeed,
    IsArchived
FROM dbo.ShipType
/**where**/
");

            query?.Invoke(builder);

            using (var db = DbFactory.OpenDbConnection())
            {
                var data = db.Query<ShipTypeDocument>(selector.RawSql, selector.Parameters);

                return new Paged<ShipTypeDocument>(data);
            }
        }
    }
}
