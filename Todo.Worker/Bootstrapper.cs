using Microsoft.WindowsAzure.ServiceRuntime;
using StructureMap;
using StructureMap.Graph;
using Todo.Command;
using Todo.CommandHandlers;

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
                    scan.WithDefaultConventions();
                });
                x.For<ICommandHandler<CreateTodoList>>()
                    .Use<CreateTodoListCommandHandler>();

                var description = RoleEnvironment.GetConfigurationSettingValue("Microsoft.ServiceBus.ConnectionString");                
            });            
        }
    }
}