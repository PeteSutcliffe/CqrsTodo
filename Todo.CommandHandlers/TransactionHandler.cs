using System;
using Todo.Command;

namespace Todo.CommandHandlers
{
    public class TransactionHandler<TCommand, TCommandHandler>
        where TCommandHandler : BaseCommandHandler, ICommandHandler<TCommand>
        where TCommand : ICommand
    {        
        public void Execute(TCommand command, TCommandHandler commandHandler)
        {
            try
            {
                commandHandler.Handle(command);
                commandHandler.Commit();
            }
            catch (Exception)
            {
                //to do...
                throw;
            }
        }
    }
}
