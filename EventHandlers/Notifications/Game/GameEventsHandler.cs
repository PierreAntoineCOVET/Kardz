using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repositories.ReadEntities;
using Repositories.ReadRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Notifications.Game
{
    public class GameEventsHandler :
        INotificationHandler<GameCreatedEvent>,
        INotificationHandler<ShuffledCardsEvent>
    {
        private readonly IGenericRepository<CoincheGame> GameRepository;
        //private readonly IGenericRepository<CoincheTeam> TeamRepository;
        //private readonly IGenericRepository<CoinchePlayer> PlayerRepository;

        public GameEventsHandler(IGenericRepository<CoincheGame> gameRepository) 
            //IGenericRepository<CoincheTeam> teamRepository, IGenericRepository<CoinchePlayer> playerRepository)
        {
            GameRepository = gameRepository;
        }

        public async Task Handle(GameCreatedEvent notification, CancellationToken cancellationToken)
        {
            var coincheGame = new CoincheGame
            {
                Id = notification.GameId,
                IsFinished = false,
                Teams = notification.Teams.Select(t => new CoincheTeam
                {
                    Number = t.Number,
                    Score = 0,
                    Players = t.Players.Select(p => new CoinchePlayer
                    {
                        Id = p.Id,
                        
                    }).ToList()
                }).ToList()
            };

            await GameRepository.Add(coincheGame);

            await GameRepository.SaveChanges();

        }

        public async Task Handle(ShuffledCardsEvent notification, CancellationToken cancellationToken)
        {
            var game = await GameRepository.Get(notification.Id);

            game.LastShuffle = String.Join(";", notification.ShuffledCards.Select(c => (int)c));

            await GameRepository.SaveChanges();
        }
    }
}
