using System;
using Cat.lexing.tokens;

namespace Cat.ast.nodes.arithmetics
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

        public static class OperationHelper
        {
            public static ArithmeticOperation GetOperationFromTokenType(ITokenType type)
            {
                return type.Name switch
                {
                    nameof(TokenTypes.Plus) => ArithmeticOperation.Plus,
                    nameof(TokenTypes.Minus) => ArithmeticOperation.Minus,
                    nameof(TokenTypes.Star) => ArithmeticOperation.Multiply,
                    nameof(TokenTypes.Divide) => ArithmeticOperation.Divide,
                    nameof(TokenTypes.Equals) => ArithmeticOperation.Equals,
                    nameof(TokenTypes.NotEquals) => ArithmeticOperation.NotEquals,
                    nameof(TokenTypes.And) => ArithmeticOperation.And,
                    nameof(TokenTypes.Or) => ArithmeticOperation.Or,
                    nameof(TokenTypes.At) => ArithmeticOperation.At,
                    nameof(TokenTypes.Hash) => ArithmeticOperation.Hash,
                    nameof(TokenTypes.Circumflex) => ArithmeticOperation.Power,
                    nameof(TokenTypes.ExclamationMark) => ArithmeticOperation.Not,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        public enum ArithmeticOperation
        {
            Plus,
            Minus,
            Multiply,
            Divide,
            Equals,
            NotEquals,
            And,
            Or,
            At,
            Hash,
            Power,
            Not,
        }
    }
}