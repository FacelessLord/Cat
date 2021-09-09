using CatApi.interpreting;

namespace CatImplementations.ast.nodes
{
    public class VariableStatementNode : INode
    {
        public INode Id { get; }
        public INode Expression { get; }
        public INode Type { get; }

        public VariableStatementNode(INode id, INode expression = null, INode type = null)
        {
            Id = id;
            Expression = expression;
            Type = type;
        }
    }
}