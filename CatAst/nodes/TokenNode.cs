using Cat.lexing.tokens;

namespace Cat.ast.nodes
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