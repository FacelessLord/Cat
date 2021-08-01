namespace Cat.ast.nodes
{
    public class ValuePairNode: INode
    {
        public INode Id { get; }
        public INode Expression { get; }

        public ValuePairNode(INode id, INode expression)
        {
            Id = id;
            Expression = expression;
        }
    }
}