using DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Commands.CreateGame
{
    public class CreateGameCommand : IRequest<GameDto>
    {
    }
}
