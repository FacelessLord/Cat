using System;
using Cat.ast.api;
using Cat.ast.nodes;
using Cat.lexing.tokens;
using static Cat.ast.api.RuleHelper;
using static Cat.ast.rules.Contexts;
using static Cat.lexing.tokens.TokenTypes;

namespace Cat.ast.rules
{
    public class LiteralRule : IContext
    {
        private static readonly RuleReader Reader = RuleHelper.Reader
            .With(Chain
                .StartWith(Token(TokenTypes.String))
                .Collect(nodes => new StringNode(nodes[0] as TokenNode)))
            .With(Chain
                .StartWith(Token(Number))
                .Collect(nodes => new NumberNode(nodes[0] as TokenNode)));

        public bool TryParse(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode result)
        {
            return Reader.TryRead(tokens, onError, out result);
        }
    }
}