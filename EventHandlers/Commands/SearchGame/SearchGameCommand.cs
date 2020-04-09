using DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Commands.SearchGame
{
    public class SearchGameCommand : IRequest<bool>
    {
        public Guid PlayerId { get; set; }

        //public int GamesType { get; set; }
    }
}
