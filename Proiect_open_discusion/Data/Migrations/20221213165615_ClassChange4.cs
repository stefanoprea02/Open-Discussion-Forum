using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiectopendiscusion.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClassChange4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Subjects");
        }
    }
}
