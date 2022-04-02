﻿using Domain.Configuration;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Factories;
using Domain.Interfaces;
using DTOs;
using DTOs.Shared;
using EventHandlers.Mappers;
using EventHandlers.Notifications.Aggregate;
using EventHandlers.Repositories;
using MediatR;
using System;
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
        /// 
        /// </summary>
        private readonly GameFactory GameFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventStoreRepository">Repository.</param>
        /// <param name="mediator">Mediator service.</param>
        public SetGameContractCommandHandler(IEventStoreRepository eventStoreRepository, IMediator mediator, GameFactory gameFactory)
        {
            EventStoreRepository = eventStoreRepository;
            Mediator = mediator;
            GameFactory = gameFactory;
        }

        /// <summary>
        /// Handler.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<GameContractDto> Handle(SetGameContractCommand request, CancellationToken cancellationToken)
        {
            var gameAggregate = await EventStoreRepository.Get(request.GameId);
            var game = GameFactory.LoadFromAggregate<IGame>(gameAggregate);

            if (game == null)
            {
                throw new GameException($"Game id {request.GameId} not found.");
            }

            game.SetGameContract(request.Color, request.Value, request.PlayerId, request.Coinched ?? false);

            await Mediator.Publish(new AggregateSaveNotification
            {
                Aggregate = game
            });

            return game.ToContractDto();
        }
    }
}
