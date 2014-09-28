using Microsoft.WindowsAzure.ServiceRuntime;
using StructureMap;
using StructureMap.Graph;
using Todo.Command;
using Todo.CommandHandlers;
using Todo.Domain;
using Todo.Infrastructure.Azure;

namespace Todo.Worker
{
    public class Bootstrapper
    {
        public static void Bootstrap(IContainer container)
        {
            container.Configure(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AssemblyContainingType<CreateTodoListCommandHandler>();
                    scan.ConnectImplementationsToTypesClosing(typeof (ICommandHandler<>));
                });

                x.For<IRepository>().Use<TableStorageRepository>();
            });            
        }
    }
}