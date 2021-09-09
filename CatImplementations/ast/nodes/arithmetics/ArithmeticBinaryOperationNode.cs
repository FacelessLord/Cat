using CatApi.interpreting;

namespace CatImplementations.ast.nodes.arithmetics
{
    public class ArithmeticBinaryOperationNode : INode
    {
        public INode A { get; }
        public ArithmeticOperation Operation { get; }
        public INode B { get; }

        public ArithmeticBinaryOperationNode(INode a, TokenNode operation, INode b)
        {
            A = a;
            Operation = OperationHelper.GetOperationFromTokenType(operation.Token.Type);
            B = b;
        }
    }
}