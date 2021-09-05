using CatAst.api;
using CatAst.nodes;
using CatLexing.tokens;

namespace CatAst.rules
{
    public class TokenRule : IRule
    {
        private readonly ITokenType _type;

        public TokenRule(ITokenType type)
        {
            _type = type;
        }
        
        public INode Read(BufferedEnumerable<Token> tokens)
        {
            var depth = tokens.PushPointer();
            foreach (var token in tokens)
            {
                if (token.Type == _type)
                {
                    return new TokenNode(token);
                }

                break;
            }

            tokens.PopPointer(depth);
            tokens.ResetPointer();
            return null;
        }
    }
}