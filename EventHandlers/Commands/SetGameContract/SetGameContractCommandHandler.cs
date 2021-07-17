using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using DTOs;
using DTOs.Shared;
using EventHandlers.Notifications.Aggregate;
using EventHandlers.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Commands.SetGameContract
{
    public class SetGameContractCommandHandler : IRequestHandler<SetGameContractCommand, GameContractDto>
    {
        /// <summary>
        /// Read model generic repository.
        /// </summary>
        private readonly IEventStoreRepository EventStoreRepository;

        /// <summary>
        /// Mediator.
        /// </summary>
        private readonly IMediator Mediator;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventStoreRepository">Repository.</param>
        /// <param name="mediator">Mediator service.</param>
        public SetGameContractCommandHandler(IEventStoreRepository eventStoreRepository, IMediator mediator)
        {
            EventStoreRepository = eventStoreRepository;
            Mediator = mediator;
        }

        /// <summary>
        /// Handler.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<GameContractDto> Handle(SetGameContractCommand request, CancellationToken cancellationToken)
        {
            var game = await EventStoreRepository.Get<IGame>(request.GameId);

            if (game == null)
            {
                throw new GameException($"Game id {request.GameId} not found.");
            }

            game.SetGameContract((ColorEnum?)request.Color, request.Value, request.PlayerId, request.GameId);

            await Mediator.Publish(new AggregateSaveNotification
            {
                Aggregate = game
            });

            return game.ToContractDto();
        }
    }
}
