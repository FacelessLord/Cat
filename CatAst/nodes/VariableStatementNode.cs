namespace Cat.ast.nodes
{
    public class VariableStatementNode : INode
    {
        public INode Id { get; }
        public INode ExpressionNode { get; }
        public INode Type { get; }

        public VariableStatementNode(INode id, INode expressionNode = null, INode type = null)
        {
            Id = id;
            ExpressionNode = expressionNode;
            Type = type;
        }
    }
}