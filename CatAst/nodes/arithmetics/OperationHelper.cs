using System;
using System.Globalization;
using Cat.lexing.tokens;

namespace CatAst.nodes.arithmetics
{
    public static class OperationHelper
    {
        public static ArithmeticOperation GetOperationFromTokenType(ITokenType type)
        {
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(type.Name) switch
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