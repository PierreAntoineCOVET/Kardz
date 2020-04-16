using Domain.Domain.Implementations;
using Domain.Domain.Interfaces;
using MediatR;
using Newtonsoft.Json;
using Repositories.EventStoreEntities;
using Repositories.EventStoreRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DbAggregate = Repositories.EventStoreEntities.Aggregate;

namespace EventHandlers.Notifications.Aggregate
{
    internal class AggregateSaveNotificationHandler : INotificationHandler<AggregateSaveNotification>
    {

        private readonly IEventStoreRepository EventStoreRepository;

        private readonly IMediator Mediator;

        public AggregateSaveNotificationHandler(IEventStoreRepository eventStoreRepository, IMediator mediator)
        {
            EventStoreRepository = eventStoreRepository;
            Mediator = mediator;
        }

        public async Task Handle(AggregateSaveNotification notification, CancellationToken cancellationToken)
        {
            var aggregate = new DbAggregate
            {
                Id = notification.Aggregate.Id,
                Type = notification.Aggregate.GetType().FullName,
                Version = notification.Aggregate.Version,
                Events = new List<Event>()
            };

            foreach (var @event in notification.Aggregate.UncommittedEvents)
            {
                await Mediator.Publish((dynamic)@event);

                aggregate.Events.Add(new Event
                {
                    AggregateId = aggregate.Id,
                    Author = null,
                    Datas = JsonConvert.SerializeObject(@event),
                    Date = DateTime.Now,
                    Type = @event.GetType().FullName,
                    Id = @event.Id,
                    Version = @event.Version
                });
            }

            await EventStoreRepository.Save(aggregate);

            notification.Aggregate.UncommittedEvents.Clear();
        }
    }
}
