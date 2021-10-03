using System.Linq;
using CatApi.interpreting;

namespace CatImplementations.ast.nodes.modules
{
    public class FunctionBodyNode : INode
    {
        public INode[] Statements { get; }

        public FunctionBodyNode(INode[] nodes)
        {
            Statements = nodes.Length switch
            {
                0 => new INode[0],
                1 => new[] { nodes[0] },
                _ => (nodes[1] is FunctionBodyNode mil
                        ? new[] { nodes[0] }.Concat(mil.Statements).ToArray()
                        : new[] { nodes[0], nodes[1] })
                    .ToArray()
            };
        }
    }
}