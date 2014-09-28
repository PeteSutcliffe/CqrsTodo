using System.Collections.Generic;

namespace Todo.Domain
{
    public interface IEventProvider
    {
        ICollection<IEvent> EventsRaised { get; }
    }
}