using System.Reflection;
using Autofac;

namespace Friday.Autofac.Composition
{
    public class CompositionRoot
    {
        public ContainerBuilder GetContainerBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(Assembly.GetEntryAssembly());
            return builder;
        }


        public IContainer GetContainer()
        {
            return GetContainerBuilder().Build();
        }

    }
}