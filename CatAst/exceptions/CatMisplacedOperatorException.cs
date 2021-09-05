using System;
using CatAst.nodes;

namespace CatAst.exceptions
{
    public class CatMisplacedOperatorException : Exception
    {
        public CatMisplacedOperatorException(TokenNode token, bool isUnary)
            : base($"Token {token.Token.Type} is placed in bad position - " +
                   (isUnary ? "on one of the ends of expression" : "on the right end of expression"))
        {
        }
    }
}