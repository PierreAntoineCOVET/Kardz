using Domain.Domain.Interfaces;
using Domain.Exceptions.Game;
using Newtonsoft.Json;
using Repositories.EventStoreEntities;
using Repositories.EventStoreRepositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domain.Services
{
    /// <summary>
    /// Game service manager.
    /// </summary>
    public class GamesServices
    {
        /// <summary>
        /// Event store repository.
        /// </summary>
        private readonly IEventStoreRepository EventStoreRepository;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventStoreRepository"><see cref="IEventStoreRepository"/></param>
        public GamesServices(IEventStoreRepository eventStoreRepository)
        {
            EventStoreRepository = eventStoreRepository;
        }

        /// <summary>
        /// Persist a game in the DB.
        /// </summary>
        /// <param name="game">Game to save.</param>
        /// <returns></returns>
        public async Task AddGame(IGame game)
        {
            var eventSource = new Aggregate
            {
                Id = game.Id,
                Type = game.GetType().FullName,
                Version = 0
            };

            var @event = new Event
            {
                Author = null,
                Datas = JsonConvert.SerializeObject(game),
                Date = DateTime.Now,
                Version = 0,
                AggregateId = eventSource.Id
            };

            await EventStoreRepository.AddAggregate(eventSource);
            await EventStoreRepository.AddEvent(@event);

            await EventStoreRepository.SaveChanges();
        }

        /// <summary>
        /// Load a game from the db.
        /// </summary>
        /// <param name="gameId">Id of the game to load.</param>
        /// <returns></returns>
        public async Task<IGame> GetGame(Guid gameId)
        {
            var eventSource = await EventStoreRepository.GetAggregate(gameId);

            var gameEvent = eventSource.Events.OrderByDescending(e => e.Version).First();
            var type = Type.GetType(eventSource.Type);

            return (IGame)JsonConvert.DeserializeObject(gameEvent.Datas, type);
        }
    }
}
