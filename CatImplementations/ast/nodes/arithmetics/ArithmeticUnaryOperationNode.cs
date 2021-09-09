using CatApi.interpreting;

namespace CatImplementations.ast.nodes.arithmetics
{
    public class ArithmeticUnaryOperationNode : INode
    {
        public INode A { get; }
        public ArithmeticOperation Operation { get; }

        public ArithmeticUnaryOperationNode(INode a, TokenNode operation)
        {
            A = a;
            Operation = OperationHelper.GetOperationFromTokenType(operation.Token.Type);
        }
    }
}