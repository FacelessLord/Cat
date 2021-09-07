namespace CatModules.api
{
    public interface IModule
    {
        public IModule[] GetImportedModules();
        public IModuleMember[] GetMemberModules();
        public IModuleMember[] GetExportedModules();
        
        public string Name { get; }
        public string Path { get; }
    }
}