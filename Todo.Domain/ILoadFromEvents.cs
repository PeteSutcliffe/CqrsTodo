using System.Collections.Generic;

namespace Todo.Domain
{
    public interface ILoadFromEvents
    {
        void LoadFromEvents(IEnumerable<IEvent> events);
    }
}