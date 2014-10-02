using System;
using Todo.Domain.Events;

namespace Todo.Domain
{
    public class Todo : BaseEntity
    {
        private readonly TodoList _todoList;
        private string _action;
        private bool _isComplete;
        private bool _completed;

        public Todo(Guid id, string action, TodoList todoList)
        {
            Id = id;
            _action = action;
            _todoList = todoList;
        }

        public void SetCompleteStatus(bool isComplete)
        {
            RaiseEvent(new TodoCompletedChanged { AggregateId = _todoList.Id, Completed = isComplete, Id = Id });
        }

        protected override void RegisterEvents()
        {
            RegisterEvent<TodoCompletedChanged>(Apply);
        }

        private void Apply(TodoCompletedChanged @event)
        {
            _completed = @event.Completed;
        }
    }
}