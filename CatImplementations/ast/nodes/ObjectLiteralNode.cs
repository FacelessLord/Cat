using CatApi.interpreting;

namespace CatImplementations.ast.nodes
{
    public class ObjectLiteralNode : INode
    {
        public ValuePairNode[] Nodes { get; }

        public ObjectLiteralNode(ValuePairListNode pairsList)
        {
            Nodes = pairsList.Nodes.ToArray();
        }
    }
}