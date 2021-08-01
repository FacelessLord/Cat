using System.Collections.Generic;
using Cat.ast.nodes;
using Cat.lexing.tokens;
using static Cat.ast.api.RuleHelper;
using static Cat.lexing.tokens.TokenTypes;

namespace Cat.ast.rules
{
    public static class Rules
    {
        private static Dictionary<ITokenType, IRule> TokenRules = new();

        //todo add caching
        public static IRule Token(ITokenType tokenType) => TokenRules.ContainsKey(tokenType)
            ? TokenRules[tokenType]
            : new TokenRule(tokenType);
        
        //add new rules here
        //use Lazy if you write recursive rules
        

        public static IRule Literal = Reader
            .With(Chain
                .StartWith(Token(TokenTypes.String))
                .Collect(nodes => new StringNode(nodes[0] as TokenNode)))
            .With(Chain
                .StartWith(Token(Number))
                .Collect(nodes => new NumberNode(nodes[0] as TokenNode)));

        public static IRule Expression = Reader
            .With(Chain.StartWith(Literal).Collect(nodes => nodes[0]));

        public static IRule PipelineF = Reader
            .With(Lazy(() => Chain
                .StartWith(Token(LParen))
                .Then(Pipeline)
                .Then(Token(RParen))
                .Collect(nodes => nodes[1])))
            .With(Chain.StartWith(Expression)
                .Collect(nodes => nodes[0]));

        public static IRule PipelineT = Reader
            .With(Lazy(() => Chain
                .StartWith(PipelineF)
                .Then(Token(And))
                .Then(PipelineT)
                .Collect(nodes => new PipelineAndNode(nodes[0], nodes[2]))))
            .With(Chain.StartWith(PipelineF)
                .Collect(nodes => nodes[0]));

        public static IRule Pipeline = Reader
            .With(Lazy(() => Chain
                .StartWith(PipelineT)
                .Then(Token(Or))
                .Then(Pipeline)
                .Collect(nodes => new PipelineOrNode(nodes[0], nodes[2]))))
            .With(Chain.StartWith(PipelineT)
                .Collect(nodes => nodes[0]));

        public static IRule FunctionBody = Reader.With(Chain.StartWith(Pipeline).Collect(nodes => nodes[0]));
    }
}