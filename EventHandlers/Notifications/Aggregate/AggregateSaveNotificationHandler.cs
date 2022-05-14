using Domain.Entities.EventStoreEntities;
using Domain.Tools.Serialization;
using EventHandlers.Repositories;
using MediatR;
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
        /// Json serializer.
        /// </summary>
        private readonly JsonSerializer JsonSerializer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventStoreRepository">Event store repository.</param>
        /// <param name="mediator">Mediatr publisher.</param>
        /// <param name="jsonSerializer">Json serializer.</param>
        public AggregateSaveNotificationHandler(IEventStoreRepository eventStoreRepository, IMediator mediator, JsonSerializer jsonSerializer)
        {
            EventStoreRepository = eventStoreRepository;
            Mediator = mediator;
            JsonSerializer = jsonSerializer;
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
            var aggregate = await EventStoreRepository.Get(notification.Aggregate.Id);

            if(aggregate == null)
            {
                aggregate = new DbAggregate
                {
                    Id = notification.Aggregate.Id,
                    Type = notification.Aggregate.GetType().FullName,
                    Version = notification.Aggregate.Version,
                    Events = new List<Event>()
                };

                await EventStoreRepository.Save(aggregate);
            }

            var uncommittedDbEvents = new List<Event>(notification.Aggregate.UncommittedEvents.Count);
            foreach (var @event in notification.Aggregate.UncommittedEvents)
            {
                if(@event is INotification updateReadModelNotification)
                {
                    await Mediator.Publish(updateReadModelNotification);
                }

                var dbEvent = new Event
                {
                    AggregateId = aggregate.Id,
                    Author = null,
                    Datas = JsonSerializer.Serialize(notification.Aggregate.GetType().FullName, @event.GetType().FullName, @event),
                    Date = DateTimeOffset.Now,
                    Type = @event.GetType().FullName,
                    Id = @event.Id,
                };

                uncommittedDbEvents.Add(dbEvent);
            }

            await EventStoreRepository.Save(aggregate, uncommittedDbEvents);

            await EventStoreRepository.SaveChanges();

            notification.Aggregate.UncommittedEvents.Clear();
        }
    }
}
