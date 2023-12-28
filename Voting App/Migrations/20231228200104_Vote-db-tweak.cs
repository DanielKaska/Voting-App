using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.Migrations
{
    /// <inheritdoc />
    public partial class Votedbtweak : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_votes_users_CreatedById",
                table: "votes");

            migrationBuilder.DropIndex(
                name: "IX_votes_CreatedById",
                table: "votes");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "votes",
                newName: "CreatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "votes",
                newName: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_votes_CreatedById",
                table: "votes",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_votes_users_CreatedById",
                table: "votes",
                column: "CreatedById",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
