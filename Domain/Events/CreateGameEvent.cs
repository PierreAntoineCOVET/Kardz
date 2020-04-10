using Domain.Domain.Interfaces;
using Newtonsoft.Json;
using Repositories.EventStoreEntities;
using Repositories.EventStoreRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class CreateGameEvent : IDomainEvent
    {
        private IGame Game;

        private IEventStoreRepository EventStoreRepository;

        public CreateGameEvent(IServiceProvider serviceProvider, IGame game)
        {
            Game = game;
            EventStoreRepository = (IEventStoreRepository)serviceProvider.GetService(typeof(IEventStoreRepository));
        }

        public void Replay()
        {
            throw new NotImplementedException();
        }

        public async Task SaveToEventStore()
        {
            var aggregate = new Aggregate
            {
                Id = Game.Id,
                Type = Game.GetType().FullName,
                Version = 0
            };

            var @event = new Event
            {
                AggregateId = Game.Id,
                Author = null,
                Datas = JsonConvert.SerializeObject(Game),
                Date = DateTime.Now,
                Type = typeof(CreateGameEvent).FullName,
                Version = 0
            };

            await EventStoreRepository.AddAggregate(aggregate);
            await EventStoreRepository.AddEvent(@event);

            await EventStoreRepository.SaveChanges();
        }
    }
}
