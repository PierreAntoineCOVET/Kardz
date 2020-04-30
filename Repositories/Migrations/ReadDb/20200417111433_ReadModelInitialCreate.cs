using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repositories.Migrations.ReadDb
{
    public partial class ReadModelInitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoincheGames",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CurrentCards = table.Column<string>(unicode: false, maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoincheGames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoincheTeams",
                columns: table => new
                {
                    Number = table.Column<int>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoincheTeams", x => new { x.GameId, x.Number });
                    table.ForeignKey(
                        name: "FK_CoincheTeams_CoincheGames_GameId",
                        column: x => x.GameId,
                        principalTable: "CoincheGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoinchePlayers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Cards = table.Column<string>(unicode: false, maxLength: 23, nullable: true),
                    Number = table.Column<int>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    TeamNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinchePlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoinchePlayers_CoincheTeams_GameId_TeamNumber",
                        columns: x => new { x.GameId, x.TeamNumber },
                        principalTable: "CoincheTeams",
                        principalColumns: new[] { "GameId", "Number" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoinchePlayers_GameId_TeamNumber",
                table: "CoinchePlayers",
                columns: new[] { "GameId", "TeamNumber" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoinchePlayers");

            migrationBuilder.DropTable(
                name: "CoincheTeams");

            migrationBuilder.DropTable(
                name: "CoincheGames");
        }
    }
}
