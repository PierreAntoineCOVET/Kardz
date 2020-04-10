using Domain.Domain.Interfaces;
using Domain.Enums;
using Newtonsoft.Json;
using Repositories.EventStoreEntities;
using Repositories.EventStoreRepositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Domain.Services
{
    /// <summary>
    /// Game service manager.
    /// </summary>
    public class GamesService
    {
        /// <summary>
        /// Event store repository.
        /// </summary>
        private readonly IEventStoreRepository EventStoreRepository;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventStoreRepository"><see cref="IEventStoreRepository"/></param>
        public GamesService(IEventStoreRepository eventStoreRepository)
        {
            EventStoreRepository = eventStoreRepository;
        }

        /// <summary>
        /// Load a game from the db.
        /// </summary>
        /// <param name="gameId">Id of the game to load.</param>
        /// <returns></returns>
        public async Task<IGame> GetGame(Guid gameId)
        {
            var eventSource = await EventStoreRepository.GetAggregate(gameId);

            var type = Type.GetType(eventSource.Type);

            var game = (IGame)Activator.CreateInstance(type, eventSource.Id);

            //game.ReplayEvents(eventSource.Events);

            return game;
        }
    }
}
