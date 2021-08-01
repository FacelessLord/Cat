namespace Cat.ast.nodes
{
    public class ExpressionNode : INode
    {
        private readonly INode _expression;

        public ExpressionNode(INode expression)
        {
            _expression = expression;
        }
    }
}