using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurboProject.Migrations
{
    /// <inheritdoc />
    public partial class Sp_AnnouncesFilter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
             CREATE OR ALTER PROC sp_AnnouncedCarsFilter
                @startYear INT = NULL,
                @endYear INT = NULL,
                @startPrice DECIMAL(18, 2),
                @endPrice DECIMAL(18, 2),
                @currId INT = NULL,
                @banId INT = NULL,
                @makeId INT = NULL,
                @modelId INT = NULL,
                @pageSize INT = 10,
                @pageIndex INT = 0,
                @totalRecords INT OUTPUT
            AS
            BEGIN
                DECLARE @count INT;
            
                SELECT @count = COUNT(*) FROM Announcements AS a
                JOIN Models AS model ON a.ModelId = model.Id
                JOIN Makes AS make ON model.MakeId = make.Id
                JOIN BanTypes AS banType ON a.BanTypeId = banType.Id
                JOIN Currencies AS c ON a.CurrencyId = c.Id
                WHERE (a.Year BETWEEN @startYear AND @endYear) AND (a.Price BETWEEN @startPrice AND @endPrice AND
                    (@currId IS NULL OR c.Id = @currId) AND (@banId IS NULL OR banType.Id = @banId) AND
                    (@makeId IS NULL OR make.Id = @makeId) AND (@modelId IS NULL OR model.Id = @modelId));
            
                SELECT @totalRecords = @count;
            
                SELECT a.Id, make.MakeName, model.ModelName, a.Price, a.Year, banType.Ban, c.CurrencyCode AS Currencie,img.Image
                FROM Announcements AS a
                JOIN Models AS model ON a.ModelId = model.Id
                JOIN Makes AS make ON model.MakeId = make.Id
                JOIN BanTypes AS banType ON a.BanTypeId = banType.Id
                JOIN Currencies AS c ON a.CurrencyId = c.Id
                OUTER APPLY (
                    SELECT TOP 1 Image FROM CarImageFiles AS f WHERE f.AnnouncementId = a.Id
                ) AS img
                WHERE (a.Year BETWEEN @startYear AND @endYear) AND (a.Price BETWEEN @startPrice AND @endPrice AND
                    (@currId IS NULL OR c.Id = @currId) AND (@banId IS NULL OR banType.Id = @banId) AND
                    (@makeId IS NULL OR make.Id = @makeId) AND (@modelId IS NULL OR model.Id = @modelId))
                ORDER BY a.Id OFFSET @pageIndex * @pageSize ROWS FETCH NEXT @pageSize ROWS ONLY;
            END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"DROP PROC sp_AnnouncedCarsFilter");
        }
    }
}
