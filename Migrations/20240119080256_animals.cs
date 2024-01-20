using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimbirSoft.Migrations
{
    /// <inheritdoc />
    public partial class animals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "visiterLocations",
                table: "Animals",
                newName: "visitedLocationsId");

            migrationBuilder.AddColumn<string>(
                name: "animalTypesId",
                table: "Animals",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "VisitedLocations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    dateTimeOfVisitLocationPoint = table.Column<DateTime>(type: "TEXT", nullable: false),
                    locationPointId = table.Column<long>(type: "INTEGER", nullable: false),
                    AnimalId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitedLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitedLocations_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VisitedLocations_Locations_locationPointId",
                        column: x => x.locationPointId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitedLocations_AnimalId",
                table: "VisitedLocations",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitedLocations_locationPointId",
                table: "VisitedLocations",
                column: "locationPointId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitedLocations");

            migrationBuilder.DropColumn(
                name: "animalTypesId",
                table: "Animals");

            migrationBuilder.RenameColumn(
                name: "visitedLocationsId",
                table: "Animals",
                newName: "visiterLocations");
        }
    }
}
