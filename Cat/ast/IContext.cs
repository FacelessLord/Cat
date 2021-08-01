using System;
using Cat.ast.api;
using Cat.lexing.tokens;

namespace Cat.ast
{
    public interface IContext
    {
        public bool TryParse(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode result);
    }
}