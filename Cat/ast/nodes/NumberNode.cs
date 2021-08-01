using System;
using Cat.lexing.tokens;

namespace Cat.ast.nodes
{
    public class NumberNode : INode
    {
        private readonly string _numberToken;

        public NumberNode(TokenNode numberToken)
        {
            if (numberToken.Token.Type != TokenTypes.Number)
                throw new ArgumentException("Token have to have type Number, not " + numberToken.Token.Type);
            _numberToken = numberToken.Token.Value;
        }
    }
}