using System;

namespace Todo.Domain
{
    public interface IEvent
    {
        Guid AggregateId { get; set; }
    }
}