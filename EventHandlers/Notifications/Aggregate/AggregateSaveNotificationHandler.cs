using Domain.Entities.EventStoreEntities;
using EventHandlers.Repositories;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DbAggregate = Domain.Entities.EventStoreEntities.Aggregate;

namespace EventHandlers.Notifications.Aggregate
{
    /// <summary>
    /// Notification handler for <see cref="AggregateSaveNotification"/>.
    /// </summary>
    internal class AggregateSaveNotificationHandler : INotificationHandler<AggregateSaveNotification>
    {
        /// <summary>
        /// Event store repository.
        /// </summary>
        private readonly IEventStoreRepository EventStoreRepository;

        /// <summary>
        /// Mediatr publisher.
        /// </summary>
        private readonly IMediator Mediator;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventStoreRepository">Event store repository.</param>
        /// <param name="mediator">Mediatr publisher.</param>
        public AggregateSaveNotificationHandler(IEventStoreRepository eventStoreRepository, IMediator mediator)
        {
            EventStoreRepository = eventStoreRepository;
            Mediator = mediator;
        }

        /// <summary>
        /// Save the aggregate in the event store.
        /// Publish all events to be saved in the read store.
        /// </summary>
        /// <param name="notification">Aggregate to store.</param>
        /// <param name="cancellationToken">Cancelation token.</param>
        /// <returns></returns>
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
                });
            }

            await EventStoreRepository.Save(aggregate);

            notification.Aggregate.UncommittedEvents.Clear();
        }
    }
}
