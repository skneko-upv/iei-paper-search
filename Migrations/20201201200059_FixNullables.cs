using Microsoft.EntityFrameworkCore.Migrations;

namespace IEIPaperSearch.Migrations
{
    public partial class FixNullables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Issues_PublishedInId",
                table: "Submission");

            migrationBuilder.AlterColumn<string>(
                name: "Month",
                table: "Issues",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_Issues_PublishedInId",
                table: "Submission",
                column: "PublishedInId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Issues_PublishedInId",
                table: "Submission");

            migrationBuilder.AlterColumn<int>(
                name: "Month",
                table: "Issues",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_Issues_PublishedInId",
                table: "Submission",
                column: "PublishedInId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
