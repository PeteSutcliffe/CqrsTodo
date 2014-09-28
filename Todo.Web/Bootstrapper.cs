using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using StructureMap;
using StructureMap.Graph;
using Todo.Command.Infrastructure;
using Todo.Infrastructure.Azure;

namespace Todo.Web
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

                var description = RoleEnvironment.GetConfigurationSettingValue("Microsoft.ServiceBus.ConnectionString");
                x.For<ICommandBus>().Use<CommandSender>().Ctor<string>().Is(description);
            });            
        }
    }
}