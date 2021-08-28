namespace Cat.di
{
    public class KernelIItemConfig<T>
    {
        private readonly Kernel _kernel;

        public KernelIItemConfig(Kernel kernel)
        {
            _kernel = kernel;
        }

        public Kernel As<T2>()
        {
            _kernel.RegisterInternal<T,T2>(null);
            return _kernel;
        }

        public Kernel As<T2>(string name)
        {
            _kernel.RegisterInternal<T, T2>(name);
            return _kernel;
        }
    }
}