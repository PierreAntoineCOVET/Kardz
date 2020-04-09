using MediatR;
using System;

namespace EventHandlers.Commands.AddPlayerTolobby
{
    public class AddPlayerToLobbyCommand : IRequest<int>
    {
        public Guid PlayerId { get; set; }
    }
}
