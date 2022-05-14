using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations.ReadDb
{
    public partial class TurnManagementImplementation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoinchePlayers_CoincheTeams_GameId_TeamNumber",
                table: "CoinchePlayers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CoincheTeams",
                table: "CoincheTeams");

            migrationBuilder.DropIndex(
                name: "IX_CoinchePlayers_GameId_TeamNumber",
                table: "CoinchePlayers");

            migrationBuilder.DropColumn(
                name: "TeamNumber",
                table: "CoinchePlayers");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "CoinchePlayers",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "CurrentPayerTurn",
                table: "CoincheGames",
                newName: "CurrentPayerNumber");

            migrationBuilder.RenameColumn(
                name: "CurrentCards",
                table: "CoincheGames",
                newName: "LastTurnCards");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "CoincheTeams",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "PlayableCards",
                table: "CoinchePlayers",
                type: "varchar(23)",
                unicode: false,
                maxLength: 23,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentTurnCards",
                table: "CoincheGames",
                type: "varchar(8)",
                unicode: false,
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoincheTeams",
                table: "CoincheTeams",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CoincheTeams_GameId",
                table: "CoincheTeams",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_CoinchePlayers_TeamId",
                table: "CoinchePlayers",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoinchePlayers_CoincheTeams_TeamId",
                table: "CoinchePlayers",
                column: "TeamId",
                principalTable: "CoincheTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoinchePlayers_CoincheTeams_TeamId",
                table: "CoinchePlayers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CoincheTeams",
                table: "CoincheTeams");

            migrationBuilder.DropIndex(
                name: "IX_CoincheTeams_GameId",
                table: "CoincheTeams");

            migrationBuilder.DropIndex(
                name: "IX_CoinchePlayers_TeamId",
                table: "CoinchePlayers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CoincheTeams");

            migrationBuilder.DropColumn(
                name: "PlayableCards",
                table: "CoinchePlayers");

            migrationBuilder.DropColumn(
                name: "CurrentTurnCards",
                table: "CoincheGames");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "CoinchePlayers",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "LastTurnCards",
                table: "CoincheGames",
                newName: "CurrentCards");

            migrationBuilder.RenameColumn(
                name: "CurrentPayerNumber",
                table: "CoincheGames",
                newName: "CurrentPayerTurn");

            migrationBuilder.AddColumn<int>(
                name: "TeamNumber",
                table: "CoinchePlayers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoincheTeams",
                table: "CoincheTeams",
                columns: new[] { "GameId", "Number" });

            migrationBuilder.CreateIndex(
                name: "IX_CoinchePlayers_GameId_TeamNumber",
                table: "CoinchePlayers",
                columns: new[] { "GameId", "TeamNumber" });

            migrationBuilder.AddForeignKey(
                name: "FK_CoinchePlayers_CoincheTeams_GameId_TeamNumber",
                table: "CoinchePlayers",
                columns: new[] { "GameId", "TeamNumber" },
                principalTable: "CoincheTeams",
                principalColumns: new[] { "GameId", "Number" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
