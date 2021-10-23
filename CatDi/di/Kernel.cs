using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CatDi.di.exceptions;

namespace CatDi.di
{
    public class Kernel
    {
        public Dictionary<Type, HashSet<IFactory>> Factories = new();

        public T Resolve<T>()
        {
            var resolver = new Resolver(this);

            var targetType = typeof(T);
            if (!Factories.ContainsKey(targetType))
                throw new CatTypeNotRegisteredException(targetType);

            return (T) Factories[targetType].First().Create(resolver);
        }

        public T Resolve<T>(string name)
        {
            var resolver = new Resolver(this);
            var targetType = typeof(T);
            if (!Factories.ContainsKey(targetType))
                throw new CatTypeNotRegisteredException(targetType);

            return (T) Factories[targetType].Single(f => f.HasName && f.Name == name).Create(resolver);
        }

        public object Resolve(Type t, Resolver resolver)
        {
            if (!Factories.ContainsKey(t))
                throw new CatTypeNotRegisteredException(t);

            return Factories[t].First().Create(resolver);
        }

        public KernelItemConfig<T> Register<T>()
        {
            return new(this);
        }

        internal void RegisterFactory<TI>(IFactory factory)
        {
            var interfaceType = typeof(TI);
            if (!Factories.ContainsKey(interfaceType))
                Factories[interfaceType] = new HashSet<IFactory>();

            Factories[interfaceType].Add(factory);
        }

        public KernelGeneratorConfig<T> Register<T>(Func<Resolver, T> func)
        {
            return new(this, func);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            var resolver = new Resolver(this);
            return Factories[typeof(T)].Select(f => f.Create(resolver)).Cast<T>();
        }

        public IEnumerable<T> FindAll<T>()
        {
            var resolver = new Resolver(this);
            return Assembly.GetAssembly(typeof(T))
                ?.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(T)) && t.IsSealed)
                .Select(f => Factory.CreateFactoryFor(f, null).Create(resolver)).Cast<T>();
        }
    }
}