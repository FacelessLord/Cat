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

        public static IRule Token(ITokenType tokenType)
        {
            if (!TokenRules.ContainsKey(tokenType))
            {
                TokenRules[tokenType] = new TokenRule(tokenType);
            }

            return TokenRules[tokenType];
        }

        //add new rules here
        //use Lazy if you write recursive rules

        public static IRule ValuePair = Reader
            .With(Lazy(() => Chain
                .StartWith(Expression)
                .Then(Token(Colon))
                .Then(Expression)
                .Collect(nodes => new ValuePairNode(nodes[0], nodes[2]))));

        public static IRule ValuePairList = Reader
            .With(Lazy(() => Chain
                .StartWith(ValuePair)
                .Then(Token(Comma))
                .Then(ValuePairList)
                .Collect(nodes => new ValuePairListNode(nodes[0] as ValuePairNode, nodes[2] as ValuePairListNode))))
            .With(Lazy(() => Chain
                .StartWith(ValuePair)
                .Collect(nodes => new ValuePairListNode(nodes[0] as ValuePairNode))));

        public static IRule ExpressionList = Reader
            .With(Lazy(() => Chain
                .StartWith(Expression)
                .Then(Token(Comma))
                .Then(ExpressionList)
                .Collect(nodes => new ExpressionListNode(nodes[0], nodes[2]))))
            .With(Lazy(() => Chain
                .StartWith(Expression)
                .Collect(nodes => new ExpressionListNode(nodes[0]))));

        public static IRule Literal = Reader
            .With(Chain
                .StartWith(Token(String))
                .Collect(nodes => new StringNode(nodes[0] as TokenNode)))
            .With(Chain
                .StartWith(Token(Number))
                .Collect(nodes => new NumberNode(nodes[0] as TokenNode)))
            .With(Chain
                .StartWith(Token(Id))
                .Collect(nodes => new IdNode(nodes[0] as TokenNode)))
            .With(Chain
                .StartWith(Token(LBracket))
                .Then(ExpressionList)
                .Then(Token(RBracket))
                .Collect(nodes => new ListNode(nodes[1] as ExpressionListNode)))
            .With(Chain
                .StartWith(Token(LBrace))
                .Then(ValuePairList)
                .Then(Token(RBrace))
                .Collect(nodes => new ObjectLiteralNode(nodes[1] as ValuePairListNode)));

        public static IRule LetVarStatement = Reader
            .With(Lazy(() => Chain
                .StartWith(Token(Let))
                .Then(Token(Id))
                .Then(Token(Set))
                .Then(Token(LParen))
                .Then(Pipeline)
                .Then(Token(RParen))
                .Collect(nodes => new VariableStatementNode(new IdNode(nodes[1] as TokenNode), nodes[4]))))
            .With(Lazy(() => Chain
                .StartWith(Token(Let))
                .Then(Token(Id))
                .Then(Token(Set))
                .Then(Expression)
                .Collect(nodes => new VariableStatementNode(new IdNode(nodes[1] as TokenNode), nodes[3]))))
            .With(Chain
                .StartWith(Token(Let))
                .Then(Token(Id))
                .Collect(nodes => new VariableStatementNode(new IdNode(nodes[1] as TokenNode))));
        
        public static IRule Expression = Reader
            .With(Chain.StartWith(Literal).Collect(nodes => nodes[0]))
            .With(Chain.StartWith(LetVarStatement).Collect(nodes => nodes[0]))
            .With(Lazy(() => Chain
                .StartWith(Token(LParen))
                .Then(Expression)
                .Then(Token(RParen))
                .Collect(nodes => nodes[1])));

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