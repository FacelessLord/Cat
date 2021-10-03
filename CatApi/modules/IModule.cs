namespace CatApi.modules
{
    public interface IModule
    {
        public IModule[] ImportedModules { get; }
        public IMember[] Members { get; }
        public IMember[] ExportedMembers { get; }

        public string Name { get; }
        public string Path { get; }
    }
}