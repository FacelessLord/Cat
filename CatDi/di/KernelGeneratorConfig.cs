using System;

namespace CatDi.di
{
    public class KernelGeneratorConfig<T>
    {
        private readonly Kernel _kernel;
        private readonly Func<Resolver, T> _generator;

        public KernelGeneratorConfig(Kernel kernel, Func<Resolver, T> generator)
        {
            _kernel = kernel;
            _generator = generator;
        }

        public Kernel As<T2>()
        {
            _kernel.RegisterFactory<T2>(new GeneratorFactory<T>(_generator));
            return _kernel;
        }

        public Kernel AsSingleton<T2>()
        {
            _kernel.RegisterFactory<T2>(new SingletonGeneratorFactory<T>(_generator));
            return _kernel;
        }

        public Kernel As<T2>(string name)
        {
            _kernel.RegisterFactory<T2>(new GeneratorFactory<T>(_generator, name));
            return _kernel;
        }

        public Kernel AsSingleton<T2>(string name)
        {
            _kernel.RegisterFactory<T2>(new SingletonGeneratorFactory<T>(_generator, name));
            return _kernel;
        }
    }
}