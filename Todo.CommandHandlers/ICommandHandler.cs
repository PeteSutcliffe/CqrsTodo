using Todo.Command;

namespace Todo.CommandHandlers
{
    public interface ICommandHandler<T> where T:ICommand
    {
        void Handle(T command);
    }
}