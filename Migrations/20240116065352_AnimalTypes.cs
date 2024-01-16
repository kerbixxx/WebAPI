using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimbirSoft.Migrations
{
    /// <inheritdoc />
    public partial class AnimalTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Types_Animals_AnimalId",
                table: "Types");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Types",
                table: "Types");

            migrationBuilder.RenameTable(
                name: "Types",
                newName: "AnimalTypes");

            migrationBuilder.RenameIndex(
                name: "IX_Types_AnimalId",
                table: "AnimalTypes",
                newName: "IX_AnimalTypes_AnimalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnimalTypes",
                table: "AnimalTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalTypes_Animals_AnimalId",
                table: "AnimalTypes",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimalTypes_Animals_AnimalId",
                table: "AnimalTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnimalTypes",
                table: "AnimalTypes");

            migrationBuilder.RenameTable(
                name: "AnimalTypes",
                newName: "Types");

            migrationBuilder.RenameIndex(
                name: "IX_AnimalTypes_AnimalId",
                table: "Types",
                newName: "IX_Types_AnimalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Types",
                table: "Types",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Types_Animals_AnimalId",
                table: "Types",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id");
        }
    }
}
