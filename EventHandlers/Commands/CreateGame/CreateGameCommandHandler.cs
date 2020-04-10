using Domain.Domain.Services;
using DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Commands.CreateGame
{
    /// <summary>
    /// Handler for <see cref="CreateGameCommand"/>.
    /// </summary>
    class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, GameDto>
    {
        /// <summary>
        /// Lobby service.
        /// </summary>
        private LobbiesService LobbiesService;

        /// <summary>
        /// Game service.
        /// </summary>
        private GamesServices GamesServices;

        /// <summary>
        /// Cosntructor.
        /// </summary>
        /// <param name="lobbiesService">Lobby service.</param>
        /// <param name="gamesServices">Game service.</param>
        public CreateGameCommandHandler(LobbiesService lobbiesService, GamesServices gamesServices)
        {
            LobbiesService = lobbiesService;
            GamesServices = gamesServices;
        }

        /// <summary>
        /// Handler for game creation.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="GameDto"/></returns>
        public async Task<GameDto> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            var game = await LobbiesService.Lobby.CreateGame();

            if (game == null)
                return null;

            await GamesServices.AddGame(game);

            return game.ToGameDto();
        }
    }
}
