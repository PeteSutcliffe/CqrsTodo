using System.Linq;
using Todo.Domain.Events;

namespace Todo.ReadModel
{
    public class ReadModelUpdater : IEventHandler<TodoListCreated>, 
        IEventHandler<TodoCreated>,
        IEventHandler<TodoCompletedChanged>
    {
        readonly TodoContext _context = new TodoContext();

        public void Handle(TodoListCreated @event)
        {
            _context.TodoLists.Add(new TodoList {Id = @event.AggregateId, Name = @event.Name});
            _context.SaveChanges();
        }

        public void Handle(TodoCreated @event)
        {
            var todoList = _context.TodoLists.Include("Todos").Single(l => l.Id == @event.AggregateId);
            todoList.Todos.Add(new Todo(){Action = @event.Action, Id = @event.TodoId});
            _context.SaveChanges();
        }

        public void Handle(TodoCompletedChanged @event)
        {
            var todoList = _context.TodoLists.Include("Todos").Single(l => l.Id == @event.AggregateId);
            var todo = todoList.Todos.Single(t => t.Id == @event.Id);
            todo.Complete = @event.Completed;
            _context.SaveChanges();
        }
    }
}
