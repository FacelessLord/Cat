using CatApi.interpreting;

namespace CatApi.ast.nodes
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