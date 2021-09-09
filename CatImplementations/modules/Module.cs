using CatApi.modules;

namespace CatImplementations.modules
{
    public class Module : IModule
    {
        public IModule[] GetImportedModules()
        {
            throw new System.NotImplementedException();
        }

        public IModuleMember[] GetMemberModules()
        {
            throw new System.NotImplementedException();
        }

        public IModuleMember[] GetExportedModules()
        {
            throw new System.NotImplementedException();
        }

        public string Name { get; }
        public string Path { get; }
    }
}