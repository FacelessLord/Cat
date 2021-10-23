namespace CatApi.modules
{
    public interface IModuleMap
    {
        public void StoreModule(IModule module);
        public bool HasModule(string name);
        public IModule GetModule(string name);
    }
}