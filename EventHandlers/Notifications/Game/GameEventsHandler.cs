using Domain.Entities.ReadEntities;
using Domain.Events;
using EventHandlers.Repositories;
using EventHandlers.Specifications;
using MediatR;
using System;
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
        INotificationHandler<ContractMadeEvent>
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
                TurnTimerBase = notification.TurnTimerBase,
                Teams = notification.Teams.Select(t => new CoincheTeam
                {
                    GameId = notification.GameId,
                    Number = t.Number,
                    Score = 0,
                    Players = t.Players.Select(p => new CoinchePlayer
                    {
                        Id = p.Id,
                        Cards = string.Join(";", p.Cards.Cast<int>()),
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
            //var games = await GenericRepository.Query(new CoincheGameDatasSpecification(notification.GameId));
            //var game = games.SingleOrDefault();

            //game.CurrentPayerTurn = notification.PlayerNumber
                
            //    new CoincheGame
            //{
            //    Id = notification.GameId,
            //    CurrentDealer = notification.CurrentDealer,
            //    CurrentPayerTurn = notification.CurrentPlayerNumber,
            //    TurnTimerBase = notification.TurnTimerBase,
            //    Teams = notification.Teams.Select(t => new CoincheTeam
            //    {
            //        GameId = notification.GameId,
            //        Number = t.Number,
            //        Score = 0,
            //        Players = t.Players.Select(p => new CoinchePlayer
            //        {
            //            Id = p.Id,
            //            Cards = String.Join(";", p.Cards.Cast<int>()),
            //            Number = p.Number,
            //            GameId = notification.GameId,
            //            TeamNumber = t.Number

            //        }).ToList()
            //    }).ToList()
            //};

            //await GenericRepository.Add(coincheGame);

            //await GenericRepository.SaveChanges();
        }
    }
}
