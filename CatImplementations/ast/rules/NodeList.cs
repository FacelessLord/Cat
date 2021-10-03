using System.Linq;
using CatApi.interpreting;

namespace CatImplementations.ast.rules
{
    public class NodeList : INode
    {
        public INode[] Nodes { get; }

        public NodeList(INode[] nodes)
        {
            Nodes = nodes;
        }

        public NodeList(INode[] nodes, NodeList list)
        {
            Nodes = nodes.Concat(list.Nodes).ToArray();
        }
    }
}