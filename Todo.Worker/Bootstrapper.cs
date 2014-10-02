using Microsoft.WindowsAzure.ServiceRuntime;
using StructureMap;
using StructureMap.Graph;
using Todo.Command;
using Todo.CommandHandlers;
using Todo.Domain;
using Todo.Domain.Infrastructure;
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

                var description = RoleEnvironment.GetConfigurationSettingValue("Microsoft.ServiceBus.ConnectionString");
                x.For<IEventBus>().Use<EventBus>().Ctor<string>().Is(description);
                x.For<IRepository>().Use<TableStorageRepository>();
            });            
        }
    }
}