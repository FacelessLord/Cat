using System;
using CatApi.interpreting;
using CatApi.lexing.tokens;

namespace CatApi.ast.nodes
{
    public class StringNode : INode
    {
        public string StringToken { get; }

        public StringNode(TokenNode stringToken)
        {
            if (stringToken.Token.Type != TokenTypes.String)
                throw new ArgumentException("Token have to have type String, not " + stringToken.Token.Type);
            StringToken = stringToken.Token.Value;
        }
    }
}