using System;
using Cat.ast.api;
using Cat.lexing.tokens;

namespace Cat.ast
{
    public interface IRule
    {
        public bool TryRead(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode result);
    }
}