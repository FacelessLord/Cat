using System;
using CatLexing.tokens;

namespace CatAst.nodes
{
    public class BoolNode : INode
    {
        public string BoolToken { get; }

        public BoolNode(TokenNode numberToken)
        {
            if (numberToken.Token.Type != TokenTypes.Bool)
                throw new ArgumentException("Token have to have type Bool, not " + numberToken.Token.Type);
            BoolToken = numberToken.Token.Value;
        }
    }
}