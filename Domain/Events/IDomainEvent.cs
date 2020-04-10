using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public interface IDomainEvent
    {
        Task SaveToEventStore();

        void Replay();
    }
}
