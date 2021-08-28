using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Cat.di.exceptions;

namespace Cat.di
{
    public class Factory
    {
        private readonly Func<Kernel, object> _generator;
        public bool HasName { get; }
        public string Name { get; }

        public Factory(Func<Kernel, object> generator, string name = null)
        {
            _generator = generator;
            Name = name;
            HasName = Name != null;
        }

        public T Create<T>(Kernel kernel)
        {
            return (T) _generator(kernel);
        }

        public static Factory CreateFactoryFor<T>([MaybeNull] string name)
        {
            return new Factory(kernel =>
            {
                var type = typeof(T);
                var ctors = type.GetConstructors();
                if (ctors.Length == 0)
                {
                    throw new CatDiNoConstructorsFoundException(type);
                }

                if (ctors.Length > 1)
                {
                    throw new CatDiTooManyConstructorsFoundException(type);
                }

                var ctor = ctors[0];

                var args = ctor.GetParameters()
                    .Select(p => p.ParameterType)
                    .Select(kernel.Resolve)
                    .ToArray();

                return ctor.Invoke(args);
            });
        }
    }
}