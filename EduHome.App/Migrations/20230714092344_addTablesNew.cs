using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHome.App.Migrations
{
    public partial class addTablesNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoLink",
                table: "NoticeBoards");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoLink",
                table: "NoticeBoards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
