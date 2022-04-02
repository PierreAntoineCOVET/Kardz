using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations.ReadDb
{
    public partial class UpdateTurnTimeoutManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TurnTimerBase",
                table: "CoincheGames",
                newName: "CurrentTurnTimeout");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentTurnTimeout",
                table: "CoincheGames",
                newName: "TurnTimerBase");
        }
    }
}
