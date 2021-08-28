using System;
using System.Collections.Generic;

namespace Cat.di
{
    public class Kernel
    {
        public Dictionary<Type, HashSet<Factory>> Factories = new();
        
        public static T ResolveStatic<T>()
        {
            throw new NotImplementedException();
        }

        public static T ResolveStatic<T>(string name)
        {
            throw new NotImplementedException();
        }
        
        public T Resolve<T>()
        {
            throw new NotImplementedException();
        }
        
        public object Resolve(Type t)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(string name)
        {
            throw new NotImplementedException();
        }

        public KernelIItemConfig<T> Register<T>()
        {
            throw new NotImplementedException();
        }

        internal void RegisterInternal<TR, TI>(string name)
        {
            var interfaceType = typeof(TI);
            if (!Factories.ContainsKey(interfaceType))
                Factories[interfaceType] = new HashSet<Factory>();

            Factories[interfaceType].Add(Factory.CreateFactoryFor<TR>(name));
        }
    }
}