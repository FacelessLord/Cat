namespace Cat.ast.nodes
{
    public class ObjectLiteralNode : INode
    {
        public ValuePairListNode PairsList { get; }

        public ObjectLiteralNode(ValuePairListNode pairsList)
        {
            PairsList = pairsList;
        }
    }
}