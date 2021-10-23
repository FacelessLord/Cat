using System;
using System.Collections.Generic;
using CatDi.di.exceptions;

namespace CatDi.di
{
    public class Resolver
    {
        private readonly Kernel _kernel;
        private HashSet<(Type, string)> typeSet = new();

        public Resolver(Kernel kernel)
        {
            _kernel = kernel;
        }

        public object Resolve(Type type, string name)
        {
            var key = (type, name);
            if (typeSet.Contains(key))
            {
                throw new CatDiCyclicImportException(type);
            }

            typeSet.Add(key);
            var dependency = _kernel.Resolve(type, this);
            typeSet.Remove(key);

            return dependency;
        }

        public object Resolve(Type type)
        {
            return Resolve(type, null);
        }

        public T Resolve<T>()
        {
            var type = typeof(T);
            return (T) Resolve(type, null);
        }

        public IEnumerable<T> FindAll<T>()
        {
            return _kernel.FindAll<T>();
        }

        public T Resolve<T>(string name)
        {
            var type = typeof(T);
            return (T) Resolve(type, name);
        }
    }
}