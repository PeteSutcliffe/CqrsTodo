using Todo.Command;
using Todo.Domain;
using Todo.Domain.Infrastructure;

namespace Todo.CommandHandlers
{
    public class CreateTodoCommandHandler : ICommandHandler<CreateTodo>
    {
        private readonly IRepository _repository;
        private readonly IEventBus _eventBus;

        public CreateTodoCommandHandler(IRepository repository, IEventBus eventBus)
        {
            _repository = repository;
            _eventBus = eventBus;
        }

        public void Handle(CreateTodo command)
        {
            var todoList = _repository.Load<TodoList>(command.AggregateId);
            todoList.CreateTodo(command.Id, command.Action);
            
            //todo: these should happen outside the command handler
            _repository.Save(todoList);
            _eventBus.Publish(todoList.EventsRaised());
        }
    }
}