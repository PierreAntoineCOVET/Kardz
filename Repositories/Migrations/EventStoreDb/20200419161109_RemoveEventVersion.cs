using Microsoft.EntityFrameworkCore.Migrations;

namespace Repositories.Migrations.EventStoreDb
{
    public partial class RemoveEventVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Events");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
