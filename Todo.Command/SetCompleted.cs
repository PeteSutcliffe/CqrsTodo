using System;

namespace Todo.Command
{
    [Serializable]
    public class SetCompleted : ICommand
    {
        public SetCompleted(Guid aggregateId, Guid id, bool completed)
        {
            AggregateId = aggregateId;
            Id = id;
            Completed = completed;
        }

        public SetCompleted()
        {
            
        }

        public Guid AggregateId { get; set; }
        public Guid Id { get; set; }
        public bool Completed { get; set; }
    }
}