using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Module = Autofac.Module;

namespace Friday.Autofac.Composition
{
    public abstract class BaseCompositionModule : Module
    {
        protected abstract void OnModuleLoaded(ContainerBuilder builder);


        private static IEnumerable<Type> GetClassesThatImplements<T>(Assembly assemblyLookIn)
        {
            var part1 = typeof(string).Assembly.GetTypes()
                .Where(t => t.IsAssignableTo<T>() && !t.IsAbstract && t.IsClass).ToList();
            part1.AddRange(assemblyLookIn.GetTypes()
                .Where(t => t.IsAssignableTo<T>() && !t.IsAbstract && t.IsClass));

            return part1;
        }

        protected void RegisterTypesThatImplements<T>(Assembly assemblyLookIn, ContainerBuilder builder,
            Action<IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>
                registrationStyle) where T : class
        {
            var typesToRegister = GetClassesThatImplements<T>(assemblyLookIn);

            foreach (var type in typesToRegister)
                registrationStyle(builder.RegisterType(type));
        }

        protected void RegisterTypeAsSingleton<T>(ContainerBuilder builder) where T : class
        {
            builder.RegisterType<T>().AsSelf().AsImplementedInterfaces().SingleInstance().ExternallyOwned();
        }
        protected void RegisterTypesAsSingletonThatImplements<T>(Assembly assemblyLookIn,ContainerBuilder builder) where T : class
        {
            RegisterTypesThatImplements<T>(assemblyLookIn,builder,
                x => x.SingleInstance().ExternallyOwned().AsSelf().AsImplementedInterfaces());
        }

        protected void RegisterTypesThatImplements<T>(Assembly assemblyLookIn,ContainerBuilder builder) where T : class
        {
            RegisterTypesThatImplements<T>(assemblyLookIn,builder, x => x.AsSelf().ExternallyOwned().AsImplementedInterfaces());
        }


        protected sealed override void Load(ContainerBuilder builder)
        {
            OnModuleLoaded(builder);
        }
    }
}
