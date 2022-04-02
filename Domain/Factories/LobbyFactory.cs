using Domain.Enums;
using Domain.Exceptions;
using Domain.GamesLogic.Coinche;
using Domain.Interfaces;

namespace Domain.Factories
{
    /// <summary>
    /// Lobby factory.
    /// </summary>
    public class LobbyFactory
    {
        /// <summary>
        /// Factory to create a player depending on the game type.
        /// </summary>
        private readonly PlayerFactory PlayerFactory;

        /// <summary>
        /// Game factory.
        /// </summary>
        private readonly GameFactory GameFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="playerFactory"></param>
        public LobbyFactory(PlayerFactory playerFactory, GameFactory gameFactory)
        {
            PlayerFactory = playerFactory;
            GameFactory = gameFactory;
        }

        /// <summary>
        /// Create lobby based on game type.
        /// </summary>
        /// <param name="gamesEnum">Game type.</param>
        /// <returns></returns>
        public ILobby CreateLobby(GamesEnum gamesEnum)
        {
            switch (gamesEnum)
            {
                case GamesEnum.Coinche:
                    return new CoincheLobby(PlayerFactory, GameFactory);

                default:
                    throw new UnknownGameTypeException(gamesEnum);
            }
        }
    }
}
