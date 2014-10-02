using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Domain.Events;
using Todo.Domain.Infrastructure;

namespace Todo.Domain
{
    public class TodoList : BaseAggregateRoute
    {
        private string _name;
        private readonly List<Todo> _todos = new List<Todo>();

        public void CreateNewList(Guid id, string name)
        {
            RaiseEvent(new TodoListCreated{AggregateId = id, Id = id, Name = name});
        }

        public void CreateTodo(Guid id, string action)
        {
            RaiseEvent(new TodoCreated() { AggregateId = Id, Id = Id, TodoId = id, Action = action });
        }

        public void MarkTodoComplete(Guid id, bool isComplete)
        {
            _todos.Single(t => t.Id == id).SetCompleteStatus(isComplete);
        }

        protected override void RegisterEvents()
        {
            RegisterEvent<TodoListCreated>(ApplyChange);
            RegisterEvent<TodoCreated>(ApplyChange);
        }

        private void ApplyChange(TodoListCreated ev)
        {
            Id = ev.AggregateId;
            _name = ev.Name;
        }

        private void ApplyChange(TodoCreated @event)
        {
            var todo = new Todo(@event.TodoId, @event.Action, this);
            RegisterChildEventProvider(todo);
            _todos.Add(todo);
        }
    }
}
