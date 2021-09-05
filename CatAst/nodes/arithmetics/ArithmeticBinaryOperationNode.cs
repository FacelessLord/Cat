using System;
using Cat.lexing.tokens;

namespace Cat.ast.nodes.arithmetics
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

        public static class OperationHelper
        {
            public static ArithmeticOperation GetOperationFromTokenType(ITokenType type)
            {
                return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(type.Name) switch
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
                    nameof(TokenTypes.Percent) => ArithmeticOperation.Percent,
                    nameof(TokenTypes.Is) => ArithmeticOperation.Is,
                    nameof(TokenTypes.As) => ArithmeticOperation.As,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }
    public enum ArithmeticOperation
    {
        Percent,
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
        Is,
        As,
    }
}