namespace Cat.ast.nodes
{
    public class ExpressionNode : INode
    {
        public INode Expression { get; }

        public ExpressionNode(INode expression)
        {
            Expression = expression;
        }
    }
}