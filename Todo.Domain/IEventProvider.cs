using System;
using System.Collections.Generic;

namespace Todo.Domain
{
    public interface IEventProvider
    {
        Guid Id { get; }
        ICollection<IEvent> EventsRaised { get; }
    }
}