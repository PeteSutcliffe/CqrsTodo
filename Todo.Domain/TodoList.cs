using System;
using Todo.Domain.Events;

namespace Todo.Domain
{
    public class TodoList : EntityBase
    {
        private string _name;

        public void CreateNewList(Guid id, string name)
        {
            RaiseEvent(new TodoListCreated{AggregateId = id, Name = name});
        }

        protected override void RegisterEvents()
        {
            RegisterEvent<TodoListCreated>(ApplyChange);
        }

        private void ApplyChange(TodoListCreated ev)
        {
            Id = ev.AggregateId;
            _name = ev.Name;
        }
    }
}
