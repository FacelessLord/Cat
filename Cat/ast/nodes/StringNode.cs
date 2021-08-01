using System;
using Cat.lexing.tokens;

namespace Cat.ast.nodes
{
    public class StringNode : INode
    {
        private readonly string _stringToken;

        public StringNode(TokenNode stringToken)
        {
            if (stringToken.Token.Type != TokenTypes.String)
                throw new ArgumentException("Token have to have type String, not " + stringToken.Token.Type);
            _stringToken = stringToken.Token.Value;
        }
    }
}