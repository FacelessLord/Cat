using System;
using Cat.ast.api;
using Cat.ast.nodes;
using Cat.lexing.tokens;

namespace Cat.ast.rules
{
    public class TokenRule : IContext
    {
        private readonly ITokenType _type;

        public TokenRule(ITokenType type)
        {
            _type = type;
        }

        public bool TryParse(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode result)
        {
            var depth = tokens.PushPointer();
            foreach (var token in tokens)
            {
                if (token.Type == _type)
                {
                    result = new TokenNode(token);
                    return true;
                }

                break;
            }

            tokens.PopPointer(depth);
            tokens.ResetPointer();
            result = null;
            return false;
        }
    }
}