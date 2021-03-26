using System;
using System.Linq;
using Dapper;
using Metrc.SampleProject.Contracts.MemberPlanets;
using Metrc.SampleProject.Services.Infrastructure;
using Metrc.SampleProject.Services.Infrastructure.Dapper;

namespace Metrc.SampleProject.Services.MemberPlanets
{
    public class MemberPlanetsRepository
    {
        private RepositoryConnectionFactory DbFactory;

        public MemberPlanetsRepository(RepositoryConnectionFactory dbFactory)
        {
            DbFactory = dbFactory;
        }

        public Paged<MemberPlanetsDocument> GetMemberPlanets(Boolean? archivedState = false)
        {
            var result = QueryMemberPlanets(builder =>
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

        public MemberPlanetsDocument GetMemberPlanetsById(Int64 id)
        {
            var result = QueryMemberPlanets(builder => { builder.Where("Id = @id", new { id }); });
            return result.Data.FirstOrDefault();
        }

        public void Create(String name, Int32 xcoordinates, Int32 ycoordinates, Int32 zcoordinates)
        {
            using (var db = DbFactory.OpenDbConnection())
            {
                db.Execute(@"
INSERT INTO dbo.MemberPlanets(
    Name,
    Xcoordinates,
    Ycoordinates,
    Zcoordinates,
    IsArchived
) VALUES (
    @name,
    @xcoordinates,
    @ycoordinates,
    @zcoordinates,
    @isArchived
);",
                    new
                    {
                        name,
                        xcoordinates,
                        ycoordinates,
                        zcoordinates,
                        isArchived = false
                    });
            }
        }

        public void Update(Int64 id, String name, Int32 xcoordinates, Int32 ycoordinates, Int32 zcoordinates)
        {
            using (var db = DbFactory.OpenDbConnection())
            {
                db.Execute(@"
UPDATE dbo.MemberPlanets
SET Name = @name,
    Xcoordinates = @xcoordinates,
    Ycoordinates = @ycoordinates,
    Zcoordinates = @zcoordinates
WHERE Id = @id;",
                    new
                    {
                        id,
                        name,
                        xcoordinates,
                        ycoordinates,
                        zcoordinates
                    });
            }
        }

        public void Archive(Int64 id)
        {
            using (var db = DbFactory.OpenDbConnection())
            {
                db.Execute("UPDATE dbo.MemberPlanets SET IsArchived = @isArchived WHERE Id = @id;", new { id, isArchived = true });
            }
        }

        public Paged<MemberPlanetsDocument> QueryMemberPlanets(Action<SqlBuilder> query = null)
        {
            var builder = new SqlBuilder();
            var selector = builder.AddTemplate(@"
SELECT
    Id,
    Name,
    Xcoordinates,
    Ycoordinates,
    Zcoordinates,
    IsArchived
FROM dbo.MemberPlanets
/**where**/
");

            query?.Invoke(builder);

            using (var db = DbFactory.OpenDbConnection())
            {
                var data = db.Query<MemberPlanetsDocument>(selector.RawSql, selector.Parameters);

                return new Paged<MemberPlanetsDocument>(data);
            }
        }
    }
}
