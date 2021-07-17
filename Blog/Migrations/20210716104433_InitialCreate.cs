using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations {
    public partial class InitialCreate : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    PostID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "varchar(150)", nullable: true),
                    Description = table.Column<string>(type: "varchar(1000)", nullable: true),
                    PublicationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    User = table.Column<string>(type: "varchar(80)", nullable: true),
                    PostID1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Post", x => x.PostID);
                    table.ForeignKey(
                        name: "FK_Post_Post_PostID",
                        column: x => x.PostID1,
                        principalTable: "Post",
                        principalColumn: "PostID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Post_PostID",
                table: "Post",
                column: "PostID1");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "Post");
        }
    }
}
