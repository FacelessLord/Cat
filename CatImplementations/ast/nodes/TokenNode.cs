using CatApi.interpreting;
using CatApi.lexing;
using CatImplementations.lexing.tokens;

namespace CatImplementations.ast.nodes
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