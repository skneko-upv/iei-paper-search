using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IEIPaperSearch.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Journals",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Surnames = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Issue",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Volume = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    JournalId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Issue_Journals_JournalId",
                        column: x => x.JournalId,
                        principalTable: "Journals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Submission",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    URL = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: true),
                    StartPage = table.Column<string>(nullable: true),
                    EndPage = table.Column<string>(nullable: true),
                    PublishedInId = table.Column<Guid>(nullable: true),
                    Publisher = table.Column<string>(nullable: true),
                    Conference = table.Column<string>(nullable: true),
                    Edition = table.Column<string>(nullable: true),
                    InProceedings_StartPage = table.Column<int>(nullable: true),
                    InProceedings_EndPage = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submission_Issue_PublishedInId",
                        column: x => x.PublishedInId,
                        principalTable: "Issue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submission_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Issue_JournalId",
                table: "Issue",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_Submission_PublishedInId",
                table: "Submission",
                column: "PublishedInId");

            migrationBuilder.CreateIndex(
                name: "IX_Submission_PersonId",
                table: "Submission",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Submission");

            migrationBuilder.DropTable(
                name: "Issue");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Journals");
        }
    }
}
