using Todo.Domain.Events;

namespace Todo.EventHandlers.ReadModel
{
    public class ReadModelUpdater : IEventHandler<TodoListCreated>
    {
        readonly TodoContext _context = new TodoContext();

        public void Handle(TodoListCreated @event)
        {
            _context.TodoLists.Add(new TodoList {Id = @event.AggregateId, Name = @event.Name});
            _context.SaveChanges();
        }
    }
}
