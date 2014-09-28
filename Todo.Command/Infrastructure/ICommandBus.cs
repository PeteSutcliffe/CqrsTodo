using System.Threading.Tasks;

namespace Todo.Command.Infrastructure
{
    public interface ICommandBus
    {
        Task SendAsync(ICommand command);
    }
}