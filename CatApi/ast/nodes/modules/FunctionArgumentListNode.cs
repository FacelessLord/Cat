using System.Linq;
using CatApi.interpreting;

namespace CatApi.ast.nodes.modules
{
    public class FunctionArgumentListNode : INode
    {
        public FunctionArgumentNode[] Arguments { get; }

        public FunctionArgumentListNode(INode[] nodes)
        {
            Arguments = nodes.Length switch
            {
                0 => new FunctionArgumentNode[0],
                1 => new[] { nodes[0] as FunctionArgumentNode },
                _ => (nodes[1] is FunctionArgumentListNode mil
                        ? new[] { nodes[0] }.Concat(mil.Arguments).ToArray()
                        : new[] { nodes[0], nodes[1] }).Cast<FunctionArgumentNode>()
                    .ToArray()
            };
        }
    }
}