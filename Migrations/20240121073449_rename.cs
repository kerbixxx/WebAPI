using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimbirSoft.Migrations
{
    /// <inheritdoc />
    public partial class rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Accounts_chipperId",
                table: "Animals",
                column: "chipperId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Accounts_chipperId",
                table: "Animals");

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
    }
}
