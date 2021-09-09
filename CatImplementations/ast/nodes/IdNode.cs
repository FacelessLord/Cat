using System;
using CatApi.interpreting;
using CatImplementations.lexing.tokens;

namespace CatImplementations.ast.nodes
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