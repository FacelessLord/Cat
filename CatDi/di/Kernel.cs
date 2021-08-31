using System;
using System.Collections.Generic;
using System.Linq;

namespace CatDi.di
{
    public class Kernel
    {
        public Dictionary<Type, HashSet<IFactory>> Factories = new();

        public T Resolve<T>()
        {
            var resolver = new Resolver(this);
            return (T) Factories[typeof(T)].First().Create(resolver);
        }

        public T Resolve<T>(string name)
        {
            var resolver = new Resolver(this);
            return (T) Factories[typeof(T)].Single(f => f.HasName && f.Name == name).Create(resolver);
        }

        public object Resolve(Type t, Resolver resolver)
        {
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
    }
}