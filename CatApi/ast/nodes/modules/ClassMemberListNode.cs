using System.Linq;
using CatApi.interpreting;
using CatApi.modules;

namespace CatApi.ast.nodes.modules
{
    public class ClassMemberListNode : INode
    {
        public IModuleObjectNode[] Members { get; }

        public ClassMemberListNode(INode[] nodes)
        {
            Members = nodes.Length switch
            {
                0 => new IModuleObjectNode[0],
                1 => new[] { nodes[0] as IModuleObjectNode },
                _ => (nodes[1] is ClassMemberListNode mil
                        ? new[] { nodes[0] }.Concat(mil.Members).ToArray()
                        : new[] { nodes[0], nodes[1] }).Cast<IModuleObjectNode>()
                    .ToArray()
            };
        }
    }
}