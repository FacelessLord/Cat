namespace CatApi.modules
{
    public interface IUnboundModule
    {
        public string[] ImportedModules { get; }
        public IMember[] Members { get; }
        public IMember[] ExportedMembers { get; }

        public string Name { get; }
        public string Path { get; }
    }
}