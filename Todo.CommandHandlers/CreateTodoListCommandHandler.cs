using Todo.Command;
using Todo.Domain;
using Todo.Domain.Infrastructure;

namespace Todo.CommandHandlers
{
    public class CreateTodoListCommandHandler : ICommandHandler<CreateTodoList>
    {
        private readonly IRepository _repository;
        private readonly IEventBus _eventBus;

        public CreateTodoListCommandHandler(IRepository repository, IEventBus eventBus)
        {
            _repository = repository;
            _eventBus = eventBus;
        }

        public void Handle(CreateTodoList command)
        {
            var newTodo = new TodoList();
            newTodo.CreateNewList(command.Id, command.Name);
            
            //todo: these should happen outside the command handler
            _repository.Save(newTodo);
            _eventBus.Publish(newTodo.EventsRaised());
        }
    }

    public class SetCompletedCommandHandler : ICommandHandler<SetCompleted>
    {
        private readonly IRepository _repository;
        private readonly IEventBus _eventBus;

        public SetCompletedCommandHandler(IRepository repository, IEventBus eventBus)
        {
            _repository = repository;
            _eventBus = eventBus;
        }

        public void Handle(SetCompleted command)
        {
            var todoList = _repository.Load<TodoList>(command.AggregateId);
            todoList.MarkTodoComplete(command.Id, command.Completed);

            //todo: these should happen outside the command handler
            _repository.Save(todoList);
            _eventBus.Publish(todoList.EventsRaised());
        }
    }
}