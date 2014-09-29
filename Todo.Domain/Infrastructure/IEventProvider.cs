using System;
using System.Collections.Generic;

namespace Todo.Domain.Infrastructure
{
    public interface IEventProvider
    {
        Guid Id { get; }
        ICollection<IEvent> EventsRaised { get; }
    }
}