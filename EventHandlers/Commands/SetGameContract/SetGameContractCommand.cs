using DTOs.Shared;
using MediatR;
using System;

namespace EventHandlers.Commands.SetGameContract
{
    public class SetGameContractCommand : IRequest<CoincheContractDto>
    {
        public int Color { get; set; }

        public int Value { get; set; }

        public Guid PlayerId { get; set; }

        public Guid GameId { get; set; }
    }
}
