using Todo.Command;
using Todo.Domain;
using Todo.Domain.Infrastructure;

namespace Todo.CommandHandlers
{
    public class SetCompletedCommandHandler : BaseCommandHandler, ICommandHandler<SetCompleted>
    {

        public SetCompletedCommandHandler(IUnitOfWork uow) : base(uow)
        {
            
        }

        public void Handle(SetCompleted command)
        {
            var todoList = UnitOfWork.Load<TodoList>(command.AggregateId);
            todoList.MarkTodoComplete(command.Id, command.Completed);            
        }
    }
}