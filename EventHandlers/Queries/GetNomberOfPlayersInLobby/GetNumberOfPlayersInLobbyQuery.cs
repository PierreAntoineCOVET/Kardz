using Domain.Enums;
using MediatR;

namespace EventHandlers.Queries.GetNomberOfPlayersInLobby
{
    /// <summary>
    /// Get the total nomber of players in the lobby.
    /// </summary>
    public class GetNumberOfPlayersInLobbyQuery: IRequest<int>
    {
        /// <summary>
        /// Game's type of the Lobby.
        /// </summary>
        public GamesEnum GameType { get; set; }
    }
}
