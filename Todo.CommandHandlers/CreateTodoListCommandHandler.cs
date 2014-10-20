using Todo.Command;
using Todo.Domain;
using Todo.Domain.Infrastructure;

namespace Todo.CommandHandlers
{
    public class CreateTodoListCommandHandler : BaseCommandHandler, ICommandHandler<CreateTodoList>
    {

        public CreateTodoListCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public void Handle(CreateTodoList command)
        {
            var newTodo = new TodoList();
            UnitOfWork.Add(newTodo);
            newTodo.CreateNewList(command.Id, command.Name);            
        }
    }
}