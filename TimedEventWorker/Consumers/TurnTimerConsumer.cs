using MassTransit;
using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimedEvents.Consumers
{
    internal class TurnTimerConsumer : IConsumer<TurnTimerMessage>
    {
        public Task Consume(ConsumeContext<TurnTimerMessage> context)
        {
            throw new NotImplementedException();
        }
    }
}
