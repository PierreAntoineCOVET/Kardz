using Domain.Entities.ReadEntities;
using Domain.Enums;
using Domain.Events;
using EventHandlers.Repositories;
using EventHandlers.Specifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Notifications.Game
{
    /// <summary>
    /// Handler for all game events to be saved in the read model.
    /// </summary>
    public class GameEventsHandler :
        INotificationHandler<GameCreatedEvent>,
        INotificationHandler<ContractMadeEvent>,
        INotificationHandler<ContractFailedEvent>
    {
        /// <summary>
        /// Read modele repository.
        /// </summary>
        private readonly IGenericRepository GenericRepository;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="genericRepository">Reand model repository.</param>
        public GameEventsHandler(IGenericRepository genericRepository) 
        {
            GenericRepository = genericRepository;
        }

        /// <summary>
        /// Handler for <see cref="GameCreatedEvent"/>.
        /// </summary>
        /// <param name="notification">Create game notification event.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        public async Task Handle(GameCreatedEvent notification, CancellationToken cancellationToken)
        {
            var coincheGame = new CoincheGame
            {
                Id = notification.GameId,
                CurrentDealer = notification.CurrentDealer,
                CurrentPayerTurn = notification.CurrentPlayerNumber,
                CurrentTurnTimeout = notification.EndOfTurn,
                Teams = notification.Teams.Select(t => new CoincheTeam
                {
                    GameId = notification.GameId,
                    Number = t.Number,
                    Score = 0,
                    Players = t.Players.Select(p => new CoinchePlayer
                    {
                        Id = p.Id,
                        Cards = CardsSerialize(p.Cards),
                        Number = p.Number,
                        GameId = notification.GameId,
                        TeamNumber = t.Number

                    }).ToList()
                }).ToList()
            };

            await GenericRepository.Add(coincheGame);

            await GenericRepository.SaveChanges();

        }
        /// <summary>
        /// TODO: Implement
        ///   - need a special event to record the new current player (not ContractMadeEvent responsability)
        ///   - need a contract read model table (or fields...)
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(ContractMadeEvent notification, CancellationToken cancellationToken)
        {
            //var games = await GenericRepository.Query(new GetCoincheGameByIdWithPlayersSpecification(notification.GameId));
            //var game = games.SingleOrDefault();

            //await GenericRepository.SaveChanges();
        }

        public async Task Handle(ContractFailedEvent notification, CancellationToken cancellationToken)
        {
            var games = await GenericRepository.Query(new GetFullCoincheGameByIdSpecification(notification.GameId));
            var game = games.SingleOrDefault();

            foreach (var player in game.Teams.SelectMany(t => t.Players))
            {
                var playerCards = notification.CardsDistribution[player.Id];
                player.Cards = CardsSerialize(playerCards);
            }

            game.CurrentDealer = notification.CurrentDealer;
            game.CurrentPayerTurn = notification.CurrentPlayerNumber;
            game.CurrentTurnTimeout = notification.EndOfTurn;

            await GenericRepository.SaveChanges();
        }

        private string CardsSerialize(IEnumerable<CardsEnum> cards)
        {
            return string.Join(";", cards.Cast<int>());
        }
    }
}
