using System;
using Cat.ast.api;
using Cat.ast.nodes;
using Cat.lexing.tokens;
using static Cat.ast.api.RuleHelper;
using static Cat.ast.rules.Contexts;
using static Cat.lexing.tokens.TokenTypes;

namespace Cat.ast.rules
{
    public class PipelineRuleT : IContext
    {
        private static readonly RuleReader Reader = RuleHelper.Reader
            .With(Chain
                .StartWith(PipelineF)
                .Then(Token(And))
                .Then(PipelineT)
                .Collect(nodes => new PipelineAndNode(nodes[0], nodes[2])))
            .With(Chain.StartWith(PipelineF)
                .Collect(nodes => nodes[0]));

        public bool TryParse(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode result)
        {
            return Reader.TryRead(tokens, onError, out result);
        }
    }
}