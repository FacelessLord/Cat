using System;
using CatLexing.tokens;

namespace CatAst.nodes
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