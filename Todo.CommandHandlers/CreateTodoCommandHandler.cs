using Todo.Command;
using Todo.Domain;
using Todo.Domain.Infrastructure;

namespace Todo.CommandHandlers
{
    public class CreateTodoCommandHandler : BaseCommandHandler, ICommandHandler<CreateTodo>
    {
        public CreateTodoCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public void Handle(CreateTodo command)
        {
            var todoList = UnitOfWork.Load<TodoList>(command.AggregateId);
            todoList.CreateTodo(command.Id, command.Action);            
        }
    }
}