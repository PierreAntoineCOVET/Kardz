using Domain.Domain.Interfaces;
using Newtonsoft.Json;
using Repositories.EventStoreEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Domain.Interfaces
{
    public static class GameAggregateExtentions
    {
        public static Aggregate ToAggregate(this IAggregate game)
        {
            var aggregate = new Aggregate
            {
                Id = game.Id,
                Type = game.GetType().FullName,
                Version = game.Version,
                Events = game.GetUncommittedEvents().Select(e => new Event
                {
                    AggregateId = game.Id,
                    Author = null,
                    Datas = JsonConvert.SerializeObject(e),
                    Date = DateTime.Now,
                    Type = e.GetType().FullName,
                    Id = e.Id,
                    Version = e.Version
                })
                .ToList()
            };

            return aggregate;
        }
    }
}
