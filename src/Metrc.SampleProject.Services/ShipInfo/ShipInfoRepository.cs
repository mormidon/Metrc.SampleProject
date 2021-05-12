using System;
using System.Linq;
using Dapper;
using Metrc.SampleProject.Contracts.ShipInfo;
using Metrc.SampleProject.Services.Infrastructure;
using Metrc.SampleProject.Services.Infrastructure.Dapper;

namespace Metrc.SampleProject.Services.ShipInfo
{
    public class ShipInfoRepository
    {
        private RepositoryConnectionFactory DbFactory;

        public ShipInfoRepository(RepositoryConnectionFactory dbFactory)
        {
            DbFactory = dbFactory;
        }

        public Paged<ShipInfoDocument> GetShipInfo(Boolean? archivedState = false)
        {
            var result = QueryShipInfo(builder =>
            {
                if (archivedState.HasValue)
                {
                    builder.Where("IsArchived = @isArchived", new { isArchived = archivedState.Value });
                }
            });

            return result;
        }

        public ShipInfoDocument GetShipInfoById(Int64 id)
        {
            var result = QueryShipInfo(builder => { builder.Where("Id = @id", new { id }); });
            return result.Data.FirstOrDefault();
        }

        public void Create(String name, Int64 occupancy, String status, Int64 shipTypeId)
        {
            using (var db = DbFactory.OpenDbConnection())
            {
                db.Execute(@"
INSERT INTO dbo.ShipInfo(
    Name,
    Occupancy,
    Status,
    ShipTypeId,
    IsArchived
) VALUES (
    @name,
    @occupancy,
    @status,
    @shipTypeId,
    @isArchived
);",
                    new
                    {
                        name,
                        occupancy,
                        status,
                        shipTypeId,
                        isArchived = false
                    });

            }
        }

        public void Update(Int64 id, String name, Int64 occupancy, String status, Int64 shipTypeId)
        {
            using (var db = DbFactory.OpenDbConnection())
            {
                db.Execute(@"
UPDATE dbo.ShipInfo
SET Name = @name,
    Occupancy = @occupancy,
    Status = @status,
    ShipTypeId = @shipTypeId
WHERE Id = @id;",
                    new
                    {
                        id,
                        name,
                        occupancy,
                        status,
                        shipTypeId
                    });
            }
        }

        public void Archive(Int64 id)
        {
            using (var db = DbFactory.OpenDbConnection())
            {
                db.Execute("UPDATE dbo.ShipInfo SET IsArchived = @isArchived WHERE Id = @id", new { id, isArchived = true });
            }
        }

        public Paged<ShipInfoDocument> QueryShipInfo(Action<SqlBuilder> query = null)
        {
            var builder = new SqlBuilder();
            var selector = builder.AddTemplate(@"
SELECT
    Id,
    Name,
    Occupancy,
    Status,
    ShipTypeId,
    IsArchived
FROM dbo.ShipInfo
/**where**/
");

            query?.Invoke(builder);

            using (var db = DbFactory.OpenDbConnection())
            {
                var data = db.Query<ShipInfoDocument>(selector.RawSql, selector.Parameters);

                return new Paged<ShipInfoDocument>(data);
            }
        }
    }
}
