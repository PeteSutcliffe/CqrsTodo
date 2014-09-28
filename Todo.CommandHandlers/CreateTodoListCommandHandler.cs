using Todo.Command;
using Todo.Domain;

namespace Todo.CommandHandlers
{
    public class CreateTodoListCommandHandler : ICommandHandler<CreateTodoList>
    {
        private readonly IRepository _repository;

        public CreateTodoListCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(CreateTodoList command)
        {
            var newTodo = new TodoList();
            newTodo.CreateNewList(command.Id, command.Name);
        }
    }
}