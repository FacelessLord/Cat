using System.Linq;
using CatApi.interpreting;
using CatApi.modules;

namespace CatImplementations.ast.nodes.modules
{
    public class ModuleObjectListNode : INode
    {
        public IModuleObjectNode[] Objects { get; }

        public ModuleObjectListNode(INode[] nodes)
        {
            Objects = nodes[1] is ModuleObjectListNode mil
                ? new[] { (IModuleObjectNode) nodes[0] }.Concat(mil.Objects).ToArray()
                : new[] { (IModuleObjectNode) nodes[0], (IModuleObjectNode) nodes[1] };
        }
    }
}