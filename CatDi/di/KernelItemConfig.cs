namespace CatDi.di
{
    public class KernelItemConfig<T>
    {
        private readonly Kernel _kernel;

        public KernelItemConfig(Kernel kernel)
        {
            _kernel = kernel;
        }

        public Kernel As<T2>()
        {
            _kernel.RegisterFactory<T2>(Factory.CreateFactoryFor<T>(null));
            return _kernel;
        }
        public Kernel AsSingleton<T2>()
        {
            _kernel.RegisterFactory<T2>(Factory.CreateSingletonFactoryFor<T>(null));
            return _kernel;
        }

        public Kernel As<T2>(string name)
        {
            _kernel.RegisterFactory<T2>(Factory.CreateFactoryFor<T>(name));
            return _kernel;
        }
        public Kernel AsSingleton<T2>(string name)
        {
            _kernel.RegisterFactory<T2>(Factory.CreateSingletonFactoryFor<T>(name));
            return _kernel;
        }
    }
}