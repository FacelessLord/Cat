namespace Cat.ast.nodes
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