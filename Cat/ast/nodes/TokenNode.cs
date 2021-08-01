using Cat.lexing.tokens;

namespace Cat.ast.nodes
{
    public class TokenNode : INode
    {
        public readonly Token Token;

        public TokenNode(Token token)
        {
            Token = token;
        }
    }
}