using System;
using System.Collections.Generic;
using Cat.ast.api;
using Cat.lexing.tokens;

namespace Cat.ast.rules
{
    public class FunctionBodyRule : IContext
    {
        public bool TryParse(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode result)
        {
            return Contexts.Pipeline.TryParse(tokens, onError, out result);
        }
    }
}