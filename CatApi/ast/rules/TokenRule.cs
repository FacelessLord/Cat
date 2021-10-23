using CatApi.ast.nodes;
using CatApi.interpreting;
using CatApi.lexing;
using CatApi.utils;

namespace CatApi.ast.rules
{
    public class TokenRule : IRule
    {
        private readonly ITokenType _type;

        public TokenRule(ITokenType type)
        {
            _type = type;
        }
        
        public INode Read(BufferedEnumerable<IToken> tokens)
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