using System.Linq;
using CatApi.interpreting;

namespace CatApi.ast.nodes.modules
{
    public class ModuleImportListNode : INode
    {
        public ModuleImportNode[] Imports { get; }

        public ModuleImportListNode(INode[] nodes)
        {
            Imports = nodes.Length switch
            {
                0 => new ModuleImportNode[0],
                1 => new[] { nodes[0] as ModuleImportNode },
                _ => (nodes[1] is ModuleImportListNode mil
                        ? new[] { nodes[0] }.Concat(mil.Imports).ToArray()
                        : new[] { nodes[0], nodes[1] }).Cast<ModuleImportNode>()
                    .ToArray()
            };
        }
    }
}