using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.Migrations
{
    /// <inheritdoc />
    public partial class _1231 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_votes_VoteId",
                table: "Answer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answer",
                table: "Answer");

            migrationBuilder.RenameTable(
                name: "Answer",
                newName: "answers");

            migrationBuilder.RenameIndex(
                name: "IX_Answer_VoteId",
                table: "answers",
                newName: "IX_answers_VoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_answers",
                table: "answers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_answers_votes_VoteId",
                table: "answers",
                column: "VoteId",
                principalTable: "votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answers_votes_VoteId",
                table: "answers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_answers",
                table: "answers");

            migrationBuilder.RenameTable(
                name: "answers",
                newName: "Answer");

            migrationBuilder.RenameIndex(
                name: "IX_answers_VoteId",
                table: "Answer",
                newName: "IX_Answer_VoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answer",
                table: "Answer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_votes_VoteId",
                table: "Answer",
                column: "VoteId",
                principalTable: "votes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
