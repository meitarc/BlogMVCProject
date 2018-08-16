using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BlogProjMeitarBorisOrel.Data.Migrations
{
    public partial class removeUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlImage",
                table: "Post");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlImage",
                table: "Post",
                nullable: true);
        }
    }
}
