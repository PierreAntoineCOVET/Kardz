using Domain.Domain.Interfaces;
using Domain.Enums;
using Newtonsoft.Json;
using Repositories.EventStoreEntities;
using Repositories.EventStoreRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class ShuffleCardsEvent : IDomainEvent
    {
        private IGame Game;

        private IEventStoreRepository EventStoreRepository;

        public ShuffleCardsEvent(IServiceProvider serviceProvider, IGame game)
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
            var @event = new Event
            {
                AggregateId = Game.Id,
                Author = null,
                Datas = JsonConvert.SerializeObject(Game.Cards),
                Date = DateTime.Now,
                Type = typeof(ShuffleCardsEvent).FullName,
                Version = 0
            };

            await EventStoreRepository.AddEvent(@event);

            await EventStoreRepository.SaveChanges();
        }
    }
}
