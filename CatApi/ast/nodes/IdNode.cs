using System;
using CatApi.interpreting;
using CatApi.lexing.tokens;

namespace CatApi.ast.nodes
{
    public class IdNode : INode
    {
        public string IdToken { get; }

        public IdNode(TokenNode idToken)
        {
            if (idToken.Token.Type != TokenTypes.Id)
                throw new ArgumentException("Token have to have type String, not " + idToken.Token.Type);
            IdToken = idToken.Token.Value;
        }
        
        public IdNode(string idToken)
        {
            IdToken = idToken;
        }
    }
}