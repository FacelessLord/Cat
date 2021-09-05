using Cat.lexing.tokens;

namespace CatAst.nodes
{
    public class TokenNode : INode
    {
        public Token Token { get; }

        public TokenNode(Token token)
        {
            Token = token;
        }
    }
}