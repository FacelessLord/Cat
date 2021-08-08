using System;
using Cat.ast.api;
using Cat.ast.nodes;
using Cat.ast.nodes.arithmetics;
using Cat.lexing.tokens;
using static Cat.ast.api.RuleHelper;
using static Cat.ast.rules.Rules;
using static Cat.lexing.tokens.TokenTypes;
using Token = Cat.lexing.tokens.Token;

namespace Cat.ast.rules
{
    public static class ArithmeticExpressionRules
    {
        public const string baseName = "Arithmetic";

        public static Func<INode[], INode> BinaryOperationCollector = nodes =>
            new ArithmeticBinaryOperationNode(nodes[0], nodes[1] as TokenNode, nodes[2]);
        public static Func<INode[], INode> UnaryOperationCollector = nodes =>
            new ArithmeticUnaryOperationNode(nodes[1], nodes[1] as TokenNode);

        public static Func<INode[], INode> IdentityCollector = nodes => nodes[0];
    
        public static Func<string, Func<ITokenType, Func<RuleReader, RuleReader>>> BinaryArithmeticOperationGenerator = name => token =>
            prevRule => Reader.Named(name)
                .With(thisRule => Chain.StartWith(prevRule).Then(Token(token)).Then(thisRule)
                    .CollectBy(BinaryOperationCollector))
                .With(Chain.StartWith(prevRule).CollectBy(IdentityCollector));
        public static Func<string, Func<ITokenType, ITokenType, Func<RuleReader, RuleReader>>> TwoBinaryArithmeticOperationGenerator = name => (token1, token2) =>
            prevRule => Reader.Named(name)
                .With(thisRule => Chain.StartWith(prevRule).Then(Token(token1)).Then(thisRule)
                    .CollectBy(BinaryOperationCollector))
                .With(thisRule => Chain.StartWith(prevRule).Then(Token(token2)).Then(thisRule)
                    .CollectBy(BinaryOperationCollector))
                .With(Chain.StartWith(prevRule).CollectBy(IdentityCollector));

        public static Func<string, Func<ITokenType, Func<RuleReader, RuleReader>>> UnaryArithmeticOperationGenerator = name => token =>
            prevRule => Reader.Named(name)
                .With(Chain.StartWith(Token(token)).Then(prevRule)
                    .CollectBy(UnaryOperationCollector))
                .With(Chain.StartWith(prevRule).CollectBy(IdentityCollector));
        public static Func<string, Func<ITokenType, ITokenType, Func<RuleReader, RuleReader>>> TwoUnaryArithmeticOperationGenerator = name => (token1, token2) =>
            prevRule => Reader.Named(name)
                .With(Chain.StartWith(Token(token1)).Then(prevRule)
                    .CollectBy(UnaryOperationCollector))
                .With(Chain.StartWith(Token(token2)).Then(prevRule)
                    .CollectBy(UnaryOperationCollector))
                .With(Chain.StartWith(prevRule).CollectBy(IdentityCollector));

        public static Func<RuleReader, RuleReader> ArithmeticOr = BinaryArithmeticOperationGenerator(nameof(ArithmeticOr))(Or);
        public static Func<RuleReader, RuleReader> ArithmeticAnd = BinaryArithmeticOperationGenerator(nameof(ArithmeticAnd))(And);
        public static Func<RuleReader, RuleReader> ArithmeticEquals = TwoBinaryArithmeticOperationGenerator(nameof(ArithmeticEquals))(TokenTypes.Equals, NotEquals);
        public static Func<RuleReader, RuleReader> ArithmeticPercent = BinaryArithmeticOperationGenerator(nameof(ArithmeticPercent))(Percent);
        public static Func<RuleReader, RuleReader> ArithmeticPlus = TwoBinaryArithmeticOperationGenerator(nameof(ArithmeticPlus))(Plus, Minus);
        public static Func<RuleReader, RuleReader> ArithmeticStar = TwoBinaryArithmeticOperationGenerator(nameof(ArithmeticStar))(Star, Divide);
        public static Func<RuleReader, RuleReader> ArithmeticNot = UnaryArithmeticOperationGenerator(nameof(ArithmeticNot))(ExclamationMark);
        public static Func<RuleReader, RuleReader> ArithmeticUPlus = TwoUnaryArithmeticOperationGenerator(nameof(ArithmeticUPlus))(Plus, Minus);
        public static Func<RuleReader, RuleReader> ArithmeticParens = prevRule => Reader.Named(nameof(ArithmeticParens))
            .With(Chain.StartWith(Token(LParen)).Then(prevRule).Then(Token(RParen))
                .CollectBy(nodes => nodes[1]))
            .With(Chain.StartWith(SimpleExpression).CollectBy(IdentityCollector));

        public static IRule ArithmeticExpression = ArithmeticRuleHelper.StartRule()
            .Named(nameof(ArithmeticExpression))
            .StartWith(ArithmeticOr)
            .Then(ArithmeticAnd)
            .Then(ArithmeticEquals)
            .Then(ArithmeticPercent)
            .Then(ArithmeticPlus)
            .Then(ArithmeticStar)
            .Then(ArithmeticNot)
            .Then(ArithmeticUPlus)
            .Then(ArithmeticParens)
            .Recurse();
    }
}