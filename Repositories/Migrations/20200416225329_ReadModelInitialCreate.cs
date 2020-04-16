using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repositories.Migrations
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
                    CurrentCards = table.Column<string>(unicode: false, maxLength: 8, nullable: true),
                    LastShuffle = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    IsFinished = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoincheGames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoincheTeams",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoincheTeams", x => x.Id);
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
                    TeamId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinchePlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoinchePlayers_CoincheTeams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "CoincheTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoinchePlayers_TeamId",
                table: "CoinchePlayers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_CoincheTeams_GameId",
                table: "CoincheTeams",
                column: "GameId");
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
