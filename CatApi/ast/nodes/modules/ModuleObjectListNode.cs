using System.Linq;
using CatApi.interpreting;
using CatApi.modules;

namespace CatApi.ast.nodes.modules
{
    public class ModuleObjectListNode : INode
    {
        public IModuleObjectNode[] Objects { get; }

        public ModuleObjectListNode(INode[] nodes)
        {
            Objects = nodes.Length switch
            {
                0 => new IModuleObjectNode[0],
                1 => new[] { nodes[0] as IModuleObjectNode },
                _ => (nodes[1] is ModuleObjectListNode mil
                        ? new[] { nodes[0] }.Concat(mil.Objects).ToArray()
                        : new[] { nodes[0], nodes[1] }).Cast<IModuleObjectNode>()
                    .ToArray()
            };
        }
    }
}