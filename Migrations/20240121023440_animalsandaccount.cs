using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimbirSoft.Migrations
{
    /// <inheritdoc />
    public partial class animalsandaccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Chipper_chipperId",
                table: "Animals");

            migrationBuilder.DropTable(
                name: "Chipper");

            migrationBuilder.RenameColumn(
                name: "chipperId",
                table: "Animals",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Animals_chipperId",
                table: "Animals",
                newName: "IX_Animals_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Accounts_AccountId",
                table: "Animals",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Accounts_AccountId",
                table: "Animals");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Animals",
                newName: "chipperId");

            migrationBuilder.RenameIndex(
                name: "IX_Animals_AccountId",
                table: "Animals",
                newName: "IX_Animals_chipperId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Chipper_chipperId",
                table: "Animals",
                column: "chipperId",
                principalTable: "Chipper",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
