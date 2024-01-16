using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimbirSoft.Migrations
{
    /// <inheritdoc />
    public partial class dbsetlocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "Animals",
                newName: "visiterLocations");

            migrationBuilder.AddColumn<int>(
                name: "chipperId",
                table: "Animals",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "chippingDateTime",
                table: "Animals",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "chippingLocationId",
                table: "Animals",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "deathDateTime",
                table: "Animals",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "gender",
                table: "Animals",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "height",
                table: "Animals",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "length",
                table: "Animals",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "lifestatus",
                table: "Animals",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "weight",
                table: "Animals",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "Chipper",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chipper", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    type = table.Column<string>(type: "TEXT", nullable: false),
                    AnimalId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Types_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animals_chipperId",
                table: "Animals",
                column: "chipperId");

            migrationBuilder.CreateIndex(
                name: "IX_Types_AnimalId",
                table: "Types",
                column: "AnimalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Chipper_chipperId",
                table: "Animals",
                column: "chipperId",
                principalTable: "Chipper",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Chipper_chipperId",
                table: "Animals");

            migrationBuilder.DropTable(
                name: "Chipper");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropIndex(
                name: "IX_Animals_chipperId",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "chipperId",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "chippingDateTime",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "chippingLocationId",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "deathDateTime",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "height",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "length",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "lifestatus",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "weight",
                table: "Animals");

            migrationBuilder.RenameColumn(
                name: "visiterLocations",
                table: "Animals",
                newName: "type");
        }
    }
}
