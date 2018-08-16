using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BlogProjMeitarBorisOrel.Data.Migrations
{
    public partial class ApplicationUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_AspNetUsers_ApplicationUserId",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Post",
                newName: "ApplicationUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Post_ApplicationUserId",
                table: "Post",
                newName: "IX_Post_ApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_AspNetUsers_ApplicationUserID",
                table: "Post",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_AspNetUsers_ApplicationUserID",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserID",
                table: "Post",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Post_ApplicationUserID",
                table: "Post",
                newName: "IX_Post_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_AspNetUsers_ApplicationUserId",
                table: "Post",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
