using System;
using Cat.ast.api;
using Cat.lexing.tokens;
using static Cat.ast.api.RuleHelper;
using static Cat.ast.rules.Contexts;

namespace Cat.ast.rules
{
    public class ExpressionRule : IContext
    {
        private static readonly RuleReader Reader = RuleHelper.Reader
            .With(Chain.StartWith(Literal).Collect(nodes => nodes[0]));

        public bool TryParse(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode result)
        {
            return Reader.TryRead(tokens, onError, out result);
        }
    }
}