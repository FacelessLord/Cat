using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CatDi.di.exceptions;

namespace CatDi.di
{
    public interface IFactory
    {
        bool HasName { get; }
        string Name { get; }
        object Create(Resolver resolver);
    }

    public class Factory : IFactory
    {
        private readonly Func<Resolver, object> _generator;
        public bool HasName { get; }
        public string Name { get; }

        public Factory(Func<Resolver, object> generator, string name = null)
        {
            _generator = generator;
            Name = name;
            HasName = Name != null;
        }

        public T Create<T>(Resolver resolver)
        {
            return (T) _generator(resolver);
        }

        public object Create(Resolver resolver)
        {
            return _generator(resolver);
        }

        public static Factory CreateFactoryFor<T>([MaybeNull] string name)
        {
            return CreateFactoryFor(typeof(T), name);
        }

        public static Factory CreateFactoryFor(Type type, [MaybeNull] string name)
        {
            return new Factory(resolver =>
            {
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
                    .Select(resolver.Resolve)
                    .ToArray();

                return ctor.Invoke(args);
            }, name);
        }

        public static SingletonFactory CreateSingletonFactoryFor<T>([MaybeNull] string name)
        {
            return CreateSingletonFactoryFor(typeof(T), name);
        }

        public static SingletonFactory CreateSingletonFactoryFor(Type type, [MaybeNull] string name)
        {
            return new SingletonFactory(kernel =>
            {
                var ctors = type.GetConstructors();
                switch (ctors.Length)
                {
                    case 0:
                        throw new CatDiNoConstructorsFoundException(type);
                    case > 1:
                        throw new CatDiTooManyConstructorsFoundException(type);
                }

                var ctor = ctors[0];

                var args = ctor.GetParameters()
                    .Select(p => p.ParameterType)
                    .Select(kernel.Resolve)
                    .ToArray();

                return ctor.Invoke(args);
            }, name);
        }

        public IFactory AsSingletonFactory()
        {
            return new SingletonFactory(_generator, Name);
        }
    }
}