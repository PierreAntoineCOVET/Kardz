using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repositories.Migrations.ReadDb
{
    public partial class AddTurnTimerBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TurnTimerBase",
                table: "CoincheGames",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TurnTimerBase",
                table: "CoincheGames");
        }
    }
}
