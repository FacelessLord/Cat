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
        public static IRule SimpleExpression = Reader
            .Named(nameof(SimpleExpression))
            .With(Lazy(() => Chain.StartWith(Literal).CollectBy(nodes => nodes[0])))
            .With(Lazy(() => Chain.StartWith(LetVarStatement).CollectBy(nodes => nodes[0])));

        public static IRule ValuePair = Reader
            .Named(nameof(ValuePair))
            .With(Lazy(() => Chain
                .StartWith(Expression)
                .Then(Token(Colon))
                .Then(ValuePair)
                .CollectBy(nodes => new ValuePairNode(nodes[0], nodes[2]))));

        public static IRule ValuePairList = Reader
            .Named(nameof(ValuePairList))
            .With(Lazy(() => Chain
                .StartWith(ValuePair)
                .Then(Token(Comma))
                .Then(ValuePairList)
                .CollectBy(nodes => new ValuePairListNode(nodes[0] as ValuePairNode, nodes[2] as ValuePairListNode))))
            .With(Lazy(() => Chain
                .StartWith(ValuePair)
                .CollectBy(nodes => new ValuePairListNode(nodes[0] as ValuePairNode))));

        public static IRule ExpressionList = Reader
            .Named(nameof(ExpressionList))
            .With(Lazy(() => Chain
                .StartWith(Expression)
                .Then(Token(Comma))
                .Then(ExpressionList)
                .CollectBy(nodes => new ExpressionListNode(nodes[0], nodes[2]))))
            .With(Lazy(() => Chain
                .StartWith(Expression)
                .CollectBy(nodes => new ExpressionListNode(nodes[0]))));

        public static IRule Literal = Reader
            .Named(nameof(Literal))
            .With(Chain
                .StartWith(Token(String))
                .CollectBy(nodes => new StringNode(nodes[0] as TokenNode)))
            .With(Chain
                .StartWith(Token(Number))
                .CollectBy(nodes => new NumberNode(nodes[0] as TokenNode)))
            .With(Chain
                .StartWith(Token(Bool))
                .CollectBy(nodes => new BoolNode(nodes[0] as TokenNode)))
            .With(Chain
                .StartWith(Token(Id))
                .CollectBy(nodes => new IdNode(nodes[0] as TokenNode)))
            .With(Chain
                .StartWith(Token(LBracket))
                .Then(ExpressionList)
                .Then(Token(RBracket))
                .CollectBy(nodes => new ListNode(nodes[1] as ExpressionListNode)))
            .With(Chain
                .StartWith(Token(LBrace))
                .Then(ValuePairList)
                .Then(Token(RBrace))
                .CollectBy(nodes => new ObjectLiteralNode(nodes[1] as ValuePairListNode)));

        public static IRule TypeName = Reader
            .Named(nameof(TypeName))
            .With(Lazy(() => Chain.StartWith(Token(Id))
                .Then(Token(Dot))
                .Then(TypeName)
                .CollectBy(nodes =>
                    new IdNode(new TokenNode(new Token(Id,
                        ((TokenNode) nodes[0]).Token.Value + "." + ((IdNode) nodes[2]).IdToken))))))
            .With(Chain.StartWith(Token(Id))
                .CollectBy(nodes =>
                    new IdNode(nodes[0] as TokenNode)));

        public static IRule LetVarStatement = Reader
            .Named(nameof(LetVarStatement))
            .With(Lazy(() => Chain
                .StartWith(Token(Let))
                .Then(Token(Id))
                .Then(Token(Colon))
                .Then(TypeName)
                .Then(Token(Set))
                .Then(Token(LParen))
                .Then(Pipeline)
                .Then(Token(RParen))
                .CollectBy(nodes => new VariableStatementNode(new IdNode(nodes[1] as TokenNode), nodes[6], nodes[3]))))
            .With(Lazy(() => Chain
                .StartWith(Token(Let))
                .Then(Token(Id))
                .Then(Token(Set))
                .Then(Token(LParen))
                .Then(Pipeline)
                .Then(Token(RParen))
                .CollectBy(nodes => new VariableStatementNode(new IdNode(nodes[1] as TokenNode), nodes[4]))))
            .With(Lazy(() => Chain
                .StartWith(Token(Let))
                .Then(Token(Id))
                .Then(Token(Colon))
                .Then(TypeName)
                .Then(Token(Set))
                .Then(Expression)
                .CollectBy(nodes => new VariableStatementNode(new IdNode(nodes[1] as TokenNode), nodes[5], nodes[3]))))
            .With(Lazy(() => Chain
                .StartWith(Token(Let))
                .Then(Token(Id))
                .Then(Token(Set))
                .Then(Expression)
                .CollectBy(nodes => new VariableStatementNode(new IdNode(nodes[1] as TokenNode), nodes[3]))))
            .With(Chain
                .StartWith(Token(Let))
                .Then(Token(Id))
                .Then(Token(Colon))
                .Then(TypeName)
                .CollectBy(nodes => new VariableStatementNode(new IdNode(nodes[1] as TokenNode), type: nodes[3])))
            .With(Chain
                .StartWith(Token(Let))
                .Then(Token(Id))
                .CollectBy(nodes => new VariableStatementNode(new IdNode(nodes[1] as TokenNode))));

        public static IRule PipelineF = Reader
            .Named(nameof(PipelineF))
            .With(Lazy(() => Chain
                .StartWith(Token(LParen))
                .Then(Pipeline)
                .Then(Token(RParen))
                .CollectBy(nodes => nodes[1])))
            .With(Lazy(() => Chain.StartWith(Expression)
                .CollectBy(nodes => nodes[0])));

        public static IRule PipelineT = Reader
            .Named(nameof(PipelineT))
            .With(Lazy(() => Chain
                .StartWith(PipelineF)
                .Then(Token(PipelineAnd))
                .Then(PipelineT)
                .CollectBy(nodes => new PipelineAndNode(nodes[0], nodes[2]))))
            .With(Chain.StartWith(PipelineF)
                .CollectBy(nodes => nodes[0]));

        public static IRule Pipeline = Reader
            .Named(nameof(Pipeline))
            .With(Lazy(() => Chain
                .StartWith(PipelineT)
                .Then(Token(PipelineOr))
                .Then(Pipeline)
                .CollectBy(nodes => new PipelineOrNode(nodes[0], nodes[2]))))
            .With(Chain.StartWith(PipelineT)
                .CollectBy(nodes => nodes[0]));

        public static IRule Expression = ArithmeticExpressionRules.ArithmeticExpression;

        public static IRule FunctionBody = Reader
            .Named(nameof(FunctionBody))
            .With(Chain
                .StartWith(Pipeline)
                .CollectBy(nodes => nodes[0]));
    }
}