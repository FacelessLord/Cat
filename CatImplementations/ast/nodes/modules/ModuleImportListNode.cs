using System.Linq;
using CatApi.interpreting;

namespace CatImplementations.ast.nodes.modules
{
    public class ModuleImportListNode : INode
    {
        public ModuleImportNode[] Imports { get; }

        public ModuleImportListNode(INode[] nodes)
        {
            Imports = nodes[1] is ModuleImportListNode mil
                ? new[] { (ModuleImportNode) nodes[0] }.Concat(mil.Imports).ToArray()
                : new[] { (ModuleImportNode) nodes[0], (ModuleImportNode) nodes[1] };
        }
    }
}