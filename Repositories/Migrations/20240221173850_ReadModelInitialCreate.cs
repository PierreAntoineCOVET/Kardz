using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class ReadModelInitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoincheGames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CurrentTurnCards = table.Column<string>(type: "TEXT", unicode: false, maxLength: 8, nullable: true),
                    LastTurnCards = table.Column<string>(type: "TEXT", unicode: false, maxLength: 8, nullable: true),
                    CurrentDealer = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentPayerNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentTurnTimeout = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoincheGames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoincheTeams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false)
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
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Cards = table.Column<string>(type: "TEXT", unicode: false, maxLength: 23, nullable: true),
                    PlayableCards = table.Column<string>(type: "TEXT", unicode: false, maxLength: 23, nullable: true),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<Guid>(type: "TEXT", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "CoincheTakes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CurrentPlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CurrentFold = table.Column<string>(type: "TEXT", unicode: false, maxLength: 23, nullable: true),
                    PreviousFold = table.Column<string>(type: "TEXT", unicode: false, maxLength: 23, nullable: true),
                    CurrentPlayerPlayableCards = table.Column<string>(type: "TEXT", unicode: false, maxLength: 23, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoincheTakes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoincheTakes_CoincheGames_GameId",
                        column: x => x.GameId,
                        principalTable: "CoincheGames",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CoincheTakes_CoinchePlayers_CurrentPlayerId",
                        column: x => x.CurrentPlayerId,
                        principalTable: "CoinchePlayers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoinchePlayers_TeamId",
                table: "CoinchePlayers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_CoincheTakes_CurrentPlayerId",
                table: "CoincheTakes",
                column: "CurrentPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_CoincheTakes_GameId",
                table: "CoincheTakes",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_CoincheTeams_GameId",
                table: "CoincheTeams",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoincheTakes");

            migrationBuilder.DropTable(
                name: "CoinchePlayers");

            migrationBuilder.DropTable(
                name: "CoincheTeams");

            migrationBuilder.DropTable(
                name: "CoincheGames");
        }
    }
}
