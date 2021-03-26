using Domain.Enums;
using Domain.Factories;

namespace Domain.GamesLogic.Coinche
{
    /// <summary>
    /// Lobby for coinche players.
    /// </summary>
    internal class CoincheLobby : LobbyBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="PlayerFactory">Player creationf factory.</param>
        public CoincheLobby(PlayerFactory PlayerFactory)
            : base(PlayerFactory)
        { }

        /// <summary>
        /// Number of player to start a new game.
        /// </summary>
        public override int NumberOfPlayerForANewGame => Consts.NUMBER_OF_PLAYERS_FOR_A_GAME;

        /// <summary>
        /// Game type.
        /// </summary>
        public override GamesEnum Game => GamesEnum.Coinche;
    }
}
