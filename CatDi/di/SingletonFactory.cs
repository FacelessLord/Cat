using System;
using CatDi.utils;

namespace CatDi.di
{
    public class SingletonFactory : IFactory
    {
        public bool HasName { get; }
        public string Name { get; }
        private LazyFunc<Resolver, object> _value { get; }

        public SingletonFactory(Func<Resolver, object> generator)
        {
            _value = new LazyFunc<Resolver, object>(generator);
        }

        public SingletonFactory(Func<Resolver, object> generator, string name)
        {
            _value = new LazyFunc<Resolver, object>(generator);
            Name = name;
            HasName = name != null;
        }

        public T Create<T>(Resolver resolver)
        {
            return (T) _value.GetValue(resolver);
        }

        public object Create(Resolver resolver)
        {
            return _value.GetValue(resolver);
        }
    }
}