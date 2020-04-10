using MediatR;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Notifications.Game
{
    public class GameEventToEventStoreNotificationHandler : INotificationHandler<GameEventNotification>
    {
        public Task Handle(GameEventNotification notification, CancellationToken cancellationToken)
        {
            return notification.DomainEvent.SaveToEventStore();
        }
    }
}
