using System;

namespace Todo.Domain.Events
{
    [Serializable]
    public class TodoCompletedChanged : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid Id { get; set; }
        public int Version { get; set; }
        public bool Completed { get; set; }
    }
}