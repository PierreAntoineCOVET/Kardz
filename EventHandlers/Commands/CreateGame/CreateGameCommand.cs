using DTOs;
using MediatR;

namespace EventHandlers.Commands.CreateGame
{
    /// <summary>
    /// Create game command.
    /// </summary>
    public class CreateGameCommand : IRequest<GameDto>
    {
        public int GameType { get; set; }
    }
}
