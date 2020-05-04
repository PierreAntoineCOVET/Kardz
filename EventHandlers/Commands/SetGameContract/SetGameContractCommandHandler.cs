using Domain.Enums;
using DTOs.Shared;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Commands.SetGameContract
{
    public class SetGameContractCommandHandler : IRequestHandler<SetGameContractCommand, CoincheContractDto>
    {
        public Task<CoincheContractDto> Handle(SetGameContractCommand request, CancellationToken cancellationToken)
        {
            var result = new CoincheContractDto
            {
                Color = request.Color,
                Value = request.Value,
                LastPlayerNumber = 0,
                CurrentPlayerNumber = 1,
                HasLastPLayerPassed = false
            };
            return Task.FromResult(result);
        }
    }
}
