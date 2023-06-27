using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations.ReadDb
{
    /// <inheritdoc />
    public partial class ReadModelTake : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoincheTakes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentPlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentFold = table.Column<string>(type: "varchar(23)", unicode: false, maxLength: 23, nullable: true),
                    PreviousFold = table.Column<string>(type: "varchar(23)", unicode: false, maxLength: 23, nullable: true),
                    CurrentPlayerPlayableCards = table.Column<string>(type: "varchar(23)", unicode: false, maxLength: 23, nullable: true)
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
                name: "IX_CoincheTakes_CurrentPlayerId",
                table: "CoincheTakes",
                column: "CurrentPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_CoincheTakes_GameId",
                table: "CoincheTakes",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoincheTakes");
        }
    }
}
