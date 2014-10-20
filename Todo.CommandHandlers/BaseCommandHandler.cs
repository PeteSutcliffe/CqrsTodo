using Todo.Domain.Infrastructure;

namespace Todo.CommandHandlers
{
    public abstract class BaseCommandHandler
    {
        protected readonly IUnitOfWork UnitOfWork;

        protected BaseCommandHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public void Commit()
        {
            UnitOfWork.Commit();
        }
    }
}