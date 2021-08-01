namespace Cat.ast.nodes
{
    public class VariableStatementNode : INode
    {
        public INode Id { get; }
        public INode ExpressionNode { get; }

        public VariableStatementNode(INode id)
        {
            Id = id;
        }

        public VariableStatementNode(INode id, INode expressionNode)
        {
            Id = id;
            ExpressionNode = expressionNode;
        }
    }
}