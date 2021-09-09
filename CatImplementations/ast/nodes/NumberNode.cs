using System;
using CatApi.interpreting;
using CatImplementations.lexing.tokens;

namespace CatImplementations.ast.nodes
{
    public class NumberNode : INode
    {
        public string NumberToken { get; }

        public NumberNode(TokenNode numberToken)
        {
            if (numberToken.Token.Type != TokenTypes.Number)
                throw new ArgumentException("Token have to have type Number, not " + numberToken.Token.Type);
            NumberToken = numberToken.Token.Value;
        }
    }
}