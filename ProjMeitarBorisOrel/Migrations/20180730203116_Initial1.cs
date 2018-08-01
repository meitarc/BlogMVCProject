using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjMeitarBorisOrel.Migrations
{
    public partial class Initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "User_Name",
                table: "User",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "User_Name",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 15);
        }
    }
}
