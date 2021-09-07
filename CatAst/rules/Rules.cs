using System;
using System.Collections.Generic;
using System.Linq;
using CatAst.api;
using CatAst.nodes;
using CatLexing.tokens;
using static CatLexing.tokens.TokenTypes;

namespace CatAst.rules
{
    public class Rules
    {
        private static Dictionary<ITokenType, IRule> TokenRules = new();

        public static Func<INode[], INode> IdentityCollector = nodes => nodes[0];
        public static IRule Token(ITokenType type)
        {
            if (!TokenRules.ContainsKey(type))
            {
                TokenRules[type] = new TokenRule(type);
            }

            return TokenRules[type];
        }

        public static IRule ID = Rule.Named(nameof(ID))
            .With(Chain.StartWith(Token(Id))
                .CollectBy(nodes => new IdNode(nodes[0] as TokenNode)));

        private static CompositeRule TypeNameRecursive = Rule.Named(nameof(TypeName))
            .With(_ => Chain.StartWith(_)
                .Then(Token(Dot))
                .Then(Token(Id))
                .CollectTailBy(nodes => nodes[1]))
            .With(Chain.StartWith(Token(Id))
                .CollectBy(nodes => nodes[0]));

        public static IRule TypeName = CompositeRule.FixLeftRecursion(TypeNameRecursive, nodes =>
            new IdNode(string.Join('.', nodes.Cast<TokenNode>().Select(t => t.Token.Value))));

        public static IRule ValuePair = Rule.Named(nameof(ValuePair))
            .With(_ => Chain.StartWith(ID)
                .Then(Token(Colon))
                .Then(Expression)
                .CollectBy(nodes => new ValuePairNode(nodes[0], nodes[2])));

        public static IRule ValuePairList = Rule.Named(nameof(ValuePairList))
            .With(_ => Chain.StartWith(ValuePair)
                .Then(Token(Comma))
                .Then(_)
                .CollectBy(nodes => new ValuePairListNode(nodes[0] as ValuePairNode, nodes[2])))
            .With(_ => Chain.StartWith(ValuePair).CollectBy(nodes => new ValuePairListNode(nodes[0] as ValuePairNode)));

        public static IRule ExpressionList = Rule.Named(nameof(ExpressionList))
            .With(_ => Chain.StartWith(Expression)
                .Then(Token(Comma))
                .Then(_)
                .CollectBy(nodes => new ExpressionListNode(nodes[0], nodes[2])))
            .With(_ => Chain.StartWith(Expression).CollectBy(nodes => new ExpressionListNode(nodes[0])));

        public static IRule Literal = Rule.Named(nameof(Literal))
            .With(Chain.StartWith(Token(Number)).CollectBy(nodes => new NumberNode(nodes[0] as TokenNode)))
            .With(Chain.StartWith(Token(TokenTypes.String)).CollectBy(nodes => new StringNode(nodes[0] as TokenNode)))
            .With(Chain.StartWith(Token(Bool)).CollectBy(nodes => new BoolNode(nodes[0] as TokenNode)))
            .With(Chain.StartWith(ID).CollectBy(IdentityCollector))
            .With(Chain.StartWith(Token(LBracket)).Then(ExpressionList).Then(Token(RBracket))
                .CollectBy(nodes => new ListNode(nodes[1] as ExpressionListNode)))
            .With(Chain.StartWith(Token(LBrace)).Then(ValuePairList).Then(Token(RBrace))
                .CollectBy(nodes => new ObjectLiteralNode(nodes[1] as ValuePairListNode)));

        public static IRule LetVarStatement = Rule.Named(nameof(LetVarStatement))
            .With(_ => Chain.StartWith(Token(Let))
                .Then(ID)
                .Then(Token(Colon))
                .Then(TypeName)
                .Then(Token(Set))
                .Then(Expression)
                .CollectBy(nodes => new VariableStatementNode(nodes[1], nodes[5], nodes[3])))
            .With(Chain.StartWith(Token(Let))
                .Then(ID)
                .Then(Token(Colon))
                .Then(TypeName)
                .CollectBy(nodes => new VariableStatementNode(nodes[1], null, nodes[3])))
            .With(_ => Chain.StartWith(Token(Let))
                .Then(ID)
                .Then(Token(Set))
                .Then(Expression)
                .CollectBy(nodes => new VariableStatementNode(nodes[1], nodes[3])));

        public static IRule SimpleExpression = Rule.Named(nameof(SimpleExpression))
            .With(Chain.StartWith(Literal)
                .CollectBy(IdentityCollector))
            .With(Chain.StartWith(LetVarStatement)
                .CollectBy(IdentityCollector));


        private static CompositeRule ArithmeticExpressionRecursive = Rule.Named(nameof(ArithmeticExpression))
            .With(_ => Chain.StartWith(_).Then(Token(Or)).Then(_)
                .CollectTailBy(nodes => new CompositeRule.NodeList(nodes)))
            .With(_ => Chain.StartWith(_).Then(Token(And)).Then(_)
                .CollectTailBy(nodes => new CompositeRule.NodeList(nodes)))
            .With(_ => Chain.StartWith(_).Then(Token(TokenTypes.Equals)).Then(_)
                .CollectTailBy(nodes => new CompositeRule.NodeList(nodes)))
            .With(_ => Chain.StartWith(_).Then(Token(NotEquals)).Then(_)
                .CollectTailBy(nodes => new CompositeRule.NodeList(nodes)))
            .With(_ => Chain.StartWith(_).Then(Token(Percent)).Then(_)
                .CollectTailBy(nodes => new CompositeRule.NodeList(nodes)))
            .With(_ => Chain.StartWith(_).Then(Token(Plus)).Then(_)
                .CollectTailBy(nodes => new CompositeRule.NodeList(nodes)))
            .With(_ => Chain.StartWith(_).Then(Token(Minus)).Then(_)
                .CollectTailBy(nodes => new CompositeRule.NodeList(nodes)))
            .With(_ => Chain.StartWith(_).Then(Token(Star)).Then(_)
                .CollectTailBy(nodes => new CompositeRule.NodeList(nodes)))
            .With(_ => Chain.StartWith(_).Then(Token(Divide)).Then(_)
                .CollectTailBy(nodes => new CompositeRule.NodeList(nodes)))
            .With(_ => Chain.StartWith(_).Then(Token(Circumflex)).Then(_)
                .CollectTailBy(nodes => new CompositeRule.NodeList(nodes)))
            .With(_ => Chain.StartWith(Token(LParen)).Then(_).Then(Token(RParen))
                .CollectBy(nodes => ArithmeticTreeCollector(((CompositeRule.NodeList) nodes[1]).Nodes)))
            .With(_ => Chain.StartWith(_).Then(Token(Is)).Then(TypeName)
                .CollectTailBy(nodes => new CompositeRule.NodeList(nodes)))
            .With(_ => Chain.StartWith(_).Then(Token(As)).Then(TypeName)
                .CollectTailBy(nodes => new CompositeRule.NodeList(nodes)))
            .With(_ => Chain.StartWith(Token(Tilda)).Then(_).CollectBy(nodes => new CompositeRule.NodeList(nodes)))
            .With(_ => Chain.StartWith(Token(Plus)).Then(_).CollectBy(nodes => new CompositeRule.NodeList(nodes)))
            .With(_ => Chain.StartWith(Token(Minus)).Then(_).CollectBy(nodes => new CompositeRule.NodeList(nodes)))
            .With(Chain.StartWith(SimpleExpression).CollectBy(IdentityCollector));


        private static Func<INode[], INode> ArithmeticTreeCollector = ArithmeticTree.Collect;

        public static IRule ArithmeticExpression =
            CompositeRule.FixLeftRecursion(ArithmeticExpressionRecursive, ArithmeticTreeCollector);

        public static IRule Expression = Rule.Named(nameof(Expression))
            .With(Chain.StartWith(ArithmeticExpression)
                .CollectBy(IdentityCollector))
            .With(Chain.StartWith(SimpleExpression)
                .CollectBy(IdentityCollector));

        public static IRule Module = ModuleRules.Module;
    }
}