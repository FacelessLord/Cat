namespace CatApi.modules
{
    public interface IModuleResolver
    {
        public IModule ResolveModuleByPath(string requestSourcePath, string path);
        public IModule ResolveModuleByName(string requestSourcePath, string name);
    }
}