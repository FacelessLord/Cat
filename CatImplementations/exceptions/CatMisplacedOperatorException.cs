using System;
using CatImplementations.ast.nodes;

namespace CatImplementations.exceptions
{
    public class CatMisplacedOperatorException : Exception
    {
        public CatMisplacedOperatorException(TokenNode token, bool isUnary)
            : base($"Token {token.Token.Type.Name} is placed in bad position - " +
                   (isUnary ? "on one of the ends of expression" : "on the right end of expression"))
        {
        }
    }
}