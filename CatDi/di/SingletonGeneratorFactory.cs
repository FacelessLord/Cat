using System;
using CatDi.utils;

namespace CatDi.di
{
    public class SingletonGeneratorFactory<T> : IFactory
    {
        private readonly LazyFunc<Resolver, T> _generator;
        public bool HasName { get; }
        public string Name { get; }

        public SingletonGeneratorFactory(Func<Resolver, T> generator, string name = null)
        {
            _generator = new LazyFunc<Resolver, T>(generator);
            Name = name;
            HasName = Name != null;
        }

        public object Create(Resolver resolver)
        {
            return _generator.GetValue(resolver);
        }
    }
}