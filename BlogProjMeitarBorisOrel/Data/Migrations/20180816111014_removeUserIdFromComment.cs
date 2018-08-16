using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BlogProjMeitarBorisOrel.Data.Migrations
{
    public partial class removeUserIdFromComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_User_UserID",
                table: "Comment");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Comment",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_User_UserID",
                table: "Comment",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_User_UserID",
                table: "Comment");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Comment",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_User_UserID",
                table: "Comment",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
