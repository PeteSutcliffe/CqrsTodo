using System.Collections.Generic;
using Todo.Domain.Events;

namespace Todo.Domain.Infrastructure
{
    public interface ILoadFromEvents
    {
        void LoadFromEvents(IEnumerable<IEvent> events);
    }
}