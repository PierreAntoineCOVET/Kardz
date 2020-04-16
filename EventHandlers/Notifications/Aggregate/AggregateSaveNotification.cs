using Domain.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Notifications.Aggregate
{
    public class AggregateSaveNotification : INotification
    {
        public IAggregate Aggregate { get; set; }
    }
}
