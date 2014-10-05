using System;

namespace Todo.Command
{
    [Serializable]
    public class CreateTodo : ICommand
    {
        public CreateTodo(Guid aggregateId, Guid id, string action)
        {
            AggregateId = aggregateId;
            Id = id;
            Action = action;
        }

        public CreateTodo()
        {
            
        }

        public Guid AggregateId { get; set; }
        public Guid Id { get; set; }
        public string Action { get; set; }
    }
}