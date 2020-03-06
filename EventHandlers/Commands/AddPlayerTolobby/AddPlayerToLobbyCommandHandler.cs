using Domain.Domain.Implementations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Commands.AddPlayerTolobby
{
    class AddPlayerToLobbyCommandHandler : IRequestHandler<AddPlayerToLobbyCommand, int>
    {
        public Task<int> Handle(AddPlayerToLobbyCommand request, CancellationToken cancellationToken)
        {
            Lobby.AddPlayer(request.Guid);

            return Task.FromResult(Lobby.NumberOfPlayers);
        }
    }
}
