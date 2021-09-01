namespace Cat.ast.nodes
{
    public class TypeCastNode : INode
    {
        public INode Value { get; }
        public INode Type { get; }

        public TypeCastNode(INode value, INode type)
        {
            Value = value;
            Type = type;
        }
    }
}