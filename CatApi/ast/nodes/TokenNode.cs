using CatApi.interpreting;
using CatApi.lexing;

namespace CatApi.ast.nodes
{
    public class TokenNode : INode
    {
        public IToken Token { get; }

        public TokenNode(IToken token)
        {
            Token = token;
        }
    }
}