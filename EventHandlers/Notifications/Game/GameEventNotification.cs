using Domain.Events;
using MediatR;

namespace EventHandlers.Notifications.Game
{
    public class GameEventNotification : INotification
    {
        public IDomainEvent DomainEvent { get; set; }
    }
}
