using CatApi.interpreting;

namespace CatImplementations.ast.nodes
{
    public class ObjectCallNode : INode
    {
        public INode Source { get; }
        public ExpressionListNode Args { get; }

        public ObjectCallNode(INode source, ExpressionListNode args)
        {
            Source = source;
            Args = args;
        }
    }
}