using System;

namespace Todo.Domain.Events
{
    public interface IEvent
    {
        Guid AggregateId { get; set; }
        Guid Id { get; set; }
        int Version { get; set; }
    }
}