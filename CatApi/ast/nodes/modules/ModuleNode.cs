using System.Linq;
using CatApi.interpreting;
using CatApi.modules;

namespace CatApi.ast.nodes.modules
{
    public class ModuleNode : INode
    {
        public string Name { get; }
        public string[] Imports { get; }

        public IModuleObjectNode[] Objects { get; }

        public ModuleNode(INode[] nodes)
        {
            var moduleDeclaration = (ModuleDeclarationNode) nodes[0];
            var moduleImports = (ModuleImportListNode) nodes[1];
            var moduleInternals = (ModuleObjectListNode) nodes[2];

            Name = moduleDeclaration.ModuleName;
            Imports = moduleImports.Imports.Select(i => i.Target.IdToken).ToArray();
            Objects = moduleInternals.Objects;
        }
    }
}