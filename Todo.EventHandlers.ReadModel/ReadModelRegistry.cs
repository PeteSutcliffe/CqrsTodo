using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using Todo.Domain.Events;

namespace Todo.ReadModel
{
    public class ReadModelRegistry : Registry
    {
        public ReadModelRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.AssemblyContainingType<ReadModelUpdater>();
                scan.ConnectImplementationsToTypesClosing(typeof(IEventHandler<>));
            });

            //not sure why above isn't registering event handlers properly
            For<IEventHandler<TodoListCreated>>().Use<ReadModelUpdater>();
            For<IEventHandler<TodoCompletedChanged>>().Use<ReadModelUpdater>();
            For<IEventHandler<TodoCreated>>().Use<ReadModelUpdater>();
        }
    }
}