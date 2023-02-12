using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiectopendiscusion.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClassChange7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Categories_CategoryId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_CategoryId",
                table: "Subjects");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_CategoryId1",
                table: "Subjects",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Categories_CategoryId1",
                table: "Subjects",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Categories_CategoryId1",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_CategoryId1",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Subjects");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Subjects",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_CategoryId",
                table: "Subjects",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Categories_CategoryId",
                table: "Subjects",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
