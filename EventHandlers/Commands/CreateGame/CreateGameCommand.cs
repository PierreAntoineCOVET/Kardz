using DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Commands.CreateGame
{
    /// <summary>
    /// Create game command.
    /// </summary>
    public class CreateGameCommand : IRequest<GameDto>
    {
    }
}
