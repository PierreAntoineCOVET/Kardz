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
    class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, GameDto>
    {
        private LobbiesService LobbiesService;
        private GamesServices GamesServices;

        public CreateGameCommandHandler(LobbiesService lobbiesService, GamesServices gamesServices)
        {
            LobbiesService = lobbiesService;
            GamesServices = gamesServices;
        }

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
