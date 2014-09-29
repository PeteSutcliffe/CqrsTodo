using System.Collections.Generic;

namespace Todo.Domain.Infrastructure
{
    public interface ILoadFromEvents
    {
        void LoadFromEvents(IEnumerable<IEvent> events);
    }
}