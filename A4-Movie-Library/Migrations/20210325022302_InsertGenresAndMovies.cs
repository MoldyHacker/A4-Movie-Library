using Microsoft.EntityFrameworkCore.Migrations;

namespace A4_Movie_Library.Migrations
{
    public partial class InsertGenresAndMovies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Migrations", "Data", @"5-InsertGenresAndMovies.sql");
            migrationBuilder.Sql(File.ReadAllText(sqlFile));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from moviegenres");
        }
    }
}
