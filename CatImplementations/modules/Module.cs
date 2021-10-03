using System.Linq;
using CatApi.modules;
using CatImplementations.ast;

namespace CatImplementations.modules
{
    public class Module : IModule
    {
        public Module(string name, string path, IModule[] importedModules, IMember[] members)
        {
            ImportedModules = importedModules;
            Members = members;
            ExportedMembers = members.Where(m => m.AccessModifier == AccessModifier.Public).ToArray();
        }


        public IModule[] ImportedModules { get; }
        public IMember[] Members { get; }
        public IMember[] ExportedMembers { get; }
        public string Name { get; }
        public string Path { get; }
    }
}