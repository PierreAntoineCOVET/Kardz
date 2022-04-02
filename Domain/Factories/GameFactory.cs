using Domain.Configuration;
using Domain.Entities.EventStoreEntities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.GamesLogic.Coinche;
using Domain.Interfaces;
using Domain.Tools.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domain.Factories
{
    /// <summary>
    /// Game factory.
    /// </summary>
    public class GameFactory
    {
        /// <summary>
        /// Coinche game configuration.
        /// </summary>
        private readonly CoincheConfiguration Configuration;

        /// <summary>
        /// Domain assembly for reflection.
        /// </summary>
        private readonly Assembly DomainAssembly;

        /// <summary>
        /// Custom json serializer.
        /// </summary>
        private readonly JsonSerializer JsonSerializer;

        public GameFactory(CoincheConfiguration configuration, JsonSerializer jsonSerializer)
        {
            Configuration = configuration;
            JsonSerializer = jsonSerializer;
            DomainAssembly = Assembly.Load("Domain");
        }

        public IGame CreateGame(GamesEnum gamesEnum, IEnumerable<IPlayer> players)
        {
            switch (gamesEnum)
            {
                case GamesEnum.Coinche:
                    return new CoincheGame(Guid.NewGuid(), players, Configuration);

                default:
                    throw new UnknownGameTypeException(gamesEnum);
            }
        }

        public T LoadFromAggregate<T>(Aggregate aggregate)
            where T : IAggregate
        {
            var aggregateInstance = (T)Activator.CreateInstance(DomainAssembly.GetType(aggregate.Type), Configuration);

            foreach (var @event in aggregate.Events.OrderBy(e => e.Date))
            {
                var eventInstance = JsonSerializer.Deserialize(aggregate.Type, @event);
                aggregateInstance.Apply((dynamic)eventInstance);
            }

            return aggregateInstance;
        }
    }
}
