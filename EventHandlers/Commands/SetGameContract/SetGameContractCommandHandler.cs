using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using DTOs.Shared;
using EventHandlers.Repositories;
using EventHandlers.Specifications;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Commands.SetGameContract
{
    public class SetGameContractCommandHandler : IRequestHandler<SetGameContractCommand, CoincheContractDto>
    {
        /// <summary>
        /// Read model generic repository.
        /// </summary>
        private readonly IEventStoreRepository StoreRepository;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="storeRepository"></param>
        public SetGameContractCommandHandler(IEventStoreRepository storeRepository)
        {
            StoreRepository = storeRepository;
        }

        public async Task<CoincheContractDto> Handle(SetGameContractCommand request, CancellationToken cancellationToken)
        {
            var game = await StoreRepository.Get<IGame>(request.GameId);

            if (game == null)
            {
                throw new GameException($"Game id {request.GameId} not found.");
            }

            var result = new CoincheContractDto
            {
                Color = request.Color,
                Value = request.Value,
                LastPlayerNumber = 0,
                CurrentPlayerNumber = 1,
                HasLastPLayerPassed = false
            };

            return result;
        }
    }
}
