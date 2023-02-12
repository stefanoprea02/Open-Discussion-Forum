using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiectopendiscusion.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClassChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Subjects");
        }
    }
}
