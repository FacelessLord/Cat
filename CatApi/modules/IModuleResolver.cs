namespace CatApi.modules
{
    public interface IModuleResolver
    {
        public IModule ResolveModule(string requestSourcePath, string name);
    }
}