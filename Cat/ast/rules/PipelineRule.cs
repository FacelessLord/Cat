using System;
using Cat.ast.api;
using Cat.ast.nodes;
using Cat.lexing.tokens;
using static Cat.ast.api.RuleHelper;
using static Cat.ast.rules.Contexts;
using static Cat.lexing.tokens.TokenTypes;

namespace Cat.ast.rules
{
    public class PipelineRule : IContext
    {
        private static readonly RuleReader Reader = RuleHelper.Reader
            .With(Chain
                .StartWith(PipelineT)
                .Then(Token(Or))
                .Then(Pipeline)
                .Collect(nodes => new PipelineOrNode(nodes[0], nodes[2])))
            .With(Chain.StartWith(PipelineT)
                .Collect(nodes => nodes[0]));

        public bool TryParse(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode result)
        {
            return Reader.TryRead(tokens, onError, out result);
        }
    }
}