using Domain.Entities.ReadEntities;
using Domain.Enums;
using Domain.Events;
using Domain.Interfaces;
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
        INotificationHandler<TurnUpdatedEvent>,
        INotificationHandler<TakeChangedEvent>
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
                Id                 = notification.GameId,
                CurrentDealer      = notification.CurrentDealer,
                CurrentPlayerNumber = notification.CurrentPlayerNumber,
                CurrentTurnTimeout = notification.EndOfTurn,
                Teams              = notification.Teams.Select(t => new CoincheTeam
                {
                    Id      = Guid.NewGuid(),
                    GameId  = notification.GameId,
                    Number  = t.Number,
                    Score   = 0,
                    Players = t.GetPlayers().Select(p => new CoinchePlayer
                    {
                        Id     = p.Id,
                        Cards  = CardsSerialize(p.GetCards()),
                        Number = p.Number

                    }).ToList()
                }).ToList()
            };

            await GenericRepository.Add(coincheGame);

            await GenericRepository.SaveChanges();

        }

        public async Task Handle(TurnUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var currentGame = await GenericRepository.GetSingleOrDefault(new GetFullCoincheGameByIdSpecification(notification.GameId));
            currentGame.CurrentPlayerNumber = notification.CurrentPlayerNumber;
            currentGame.CurrentTurnTimeout = notification.EndOfTurn;

            var currentPlayer = currentGame.Teams.SelectMany(t => t.Players).Single(p => p.Id == notification.CurrentPlayerId);
            currentPlayer.PlayableCards = CardsSerialize(notification.PlayableCards);

            await GenericRepository.Update(currentGame);
            await GenericRepository.SaveChanges();
        }

        public async Task Handle(TakeChangedEvent notification, CancellationToken cancellationToken)
        {
            var coincheTake = new CoincheTake
            {
                Id                         = Guid.NewGuid(),
                CurrentFold                = CardsSerialize(notification.CurrentFold),
                CurrentPlayerId            = notification.CurrentPlayer.Id,
                CurrentPlayerPlayableCards = CardsSerialize(notification.CurrentPlayerPlayableCards),
                GameId                     = notification.GameId,
                PreviousFold               = CardsSerialize(notification.PreviousFold)
            };

            await GenericRepository.Add(coincheTake);

            await GenericRepository.SaveChanges();
        }

        private string CardsSerialize(IEnumerable<ICards> cards)
        {
            return string.Join(";", cards.Select(c => (int)c.Card));
        }
    }
}
