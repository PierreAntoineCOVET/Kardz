using Domain.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Notifications.Aggregate
{
    /// <summary>
    /// Notification send to save an aggregate.
    /// </summary>
    public class AggregateSaveNotification : INotification
    {
        /// <summary>
        /// Aggreagte to save in the write model.
        /// </summary>
        public IAggregate Aggregate { get; set; }
    }
}
