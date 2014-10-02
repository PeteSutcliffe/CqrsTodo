using System;

namespace Todo.Domain.Events
{
    [Serializable]
    public class TodoCreated : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Action { get; set; }
        public Guid TodoId { get; set; }
    }
}