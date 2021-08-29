using System;

namespace CatDi.di
{
    public class GeneratorFactory<T> : IFactory
    {
        private readonly Func<Resolver, T> _generator;
        public bool HasName { get; }
        public string Name { get; }

        public GeneratorFactory(Func<Resolver, T> generator, string name = null)
        {
            _generator = generator;
            Name = name;
            HasName = Name != null;
        }

        public object Create(Resolver resolver)
        {
            return _generator(resolver);
        }
    }
}