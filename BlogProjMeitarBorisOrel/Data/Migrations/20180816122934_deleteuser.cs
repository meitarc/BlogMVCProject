using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BlogProjMeitarBorisOrel.Data.Migrations
{
    public partial class deleteuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_User_UserID",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_User_UserID",
                table: "Post");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Post_UserID",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Comment_UserID",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Comment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Post",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Comment",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConfirmPassword = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    First_Name = table.Column<string>(nullable: false),
                    Is_Admin = table.Column<bool>(nullable: false),
                    Last_Name = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    User_Name = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserID",
                table: "Post",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserID",
                table: "Comment",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_User_UserID",
                table: "Comment",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_User_UserID",
                table: "Post",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
