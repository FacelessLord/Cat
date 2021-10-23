using System.Collections.Generic;
using System.Linq;
using CatApi.lexing;
using CatApi.lexing.tokens;

namespace CatApi.ast.nodes.arithmetics
{
    public static class OperationHelper
    {
        public static Dictionary<ArithmeticOperation, OperationDescription> Operations;

        static OperationHelper()
        {
            Operations = new Dictionary<ArithmeticOperation, OperationDescription>();
            Operations[ArithmeticOperation.Plus] = new OperationDescription(TokenTypes.Plus, "add");
            Operations[ArithmeticOperation.Minus] = new OperationDescription(TokenTypes.Minus, "subtract");
            Operations[ArithmeticOperation.Multiply] = new OperationDescription(TokenTypes.Star, "multiply");
            Operations[ArithmeticOperation.Divide] = new OperationDescription(TokenTypes.Divide, "divide");
            Operations[ArithmeticOperation.Equals] = new OperationDescription(TokenTypes.Equals, "eq");
            Operations[ArithmeticOperation.NotEquals] = new OperationDescription(TokenTypes.NotEquals, "neq");
            Operations[ArithmeticOperation.And] = new OperationDescription(TokenTypes.And, "and");
            Operations[ArithmeticOperation.Or] = new OperationDescription(TokenTypes.Or, "or");
            Operations[ArithmeticOperation.At] = new OperationDescription(TokenTypes.At, "at");
            Operations[ArithmeticOperation.Hash] = new OperationDescription(TokenTypes.Hash, "hash");
            Operations[ArithmeticOperation.Xor] = new OperationDescription(TokenTypes.Circumflex, "xor");
            Operations[ArithmeticOperation.Percent] = new OperationDescription(TokenTypes.Percent, "mod");
            Operations[ArithmeticOperation.Is] = new OperationDescription(TokenTypes.Is, "typecheck");
            Operations[ArithmeticOperation.As] = new OperationDescription(TokenTypes.As, "typecast");
            Operations[ArithmeticOperation.UPlus] = new OperationDescription(TokenTypes.Plus, "uplus", 1);
            Operations[ArithmeticOperation.UMinus] = new OperationDescription(TokenTypes.Minus, "uminus", 1);
            Operations[ArithmeticOperation.Not] = new OperationDescription(TokenTypes.ExclamationMark, "not", 1);
        }

        public static ArithmeticOperation GetOperationFromTokenType(ITokenType type, int arity)
        {
            return Operations.Where(kv => kv.Value.TokenType == type && kv.Value.Artity == arity)
                .Select(kv => kv.Key).Single();
        }

        public static string GetOperationName(ArithmeticOperation operation)
        {
            return Operations[operation].Name;
        }
    }

    public record OperationDescription(ITokenType TokenType, string Name, int Artity = 2);
}