using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Commands.AddPlayerTolobby
{
    public class AddPlayerToLobbyCommand : IRequest<int>
    {
        public Guid Guid { get; set; }
    }
}
