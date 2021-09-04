using System.Linq;
using Cat.ast;
using Cat.ast.api;
using Cat.ast.new_ast;
using Cat.ast.nodes;
using Cat.lexing.tokens;
using FluentAssertions;
using NUnit.Framework;
using static Cat.lexing.tokens.TokenTypes;
using IRule = Cat.ast.new_ast.IRule;

namespace CatAst_Tests
{
    public class Tests
    {
        public IRule Token(ITokenType type) => new TokenRule(type);


        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SimpleTokenRule()
        {
            var rule = Token(Let);

            var token = new Token(Let, "let");
            var node = rule.Read(new BufferedEnumerable<Token>(new[] { token }));

            node.Should().BeOfType<TokenNode>();
            var tokenNode = (TokenNode) node;
            tokenNode.Token.Should().Be(token);
        }

        private class NodeList : INode
        {
            public INode[] Nodes { get; }

            public NodeList(INode[] nodes)
            {
                Nodes = nodes;
            }
        }

        [Test]
        public void LetVarTokenRule()
        {
            var rule = Rule.Named("testRule")
                .With(Chain.StartWith(Token(Let))
                    .Then(Token(Id))
                    .Then(Token(Colon))
                    .Then(Token(Id))
                    .CollectBy(nodes => new NodeList(nodes)));

            var let = new Token(Let, "let");
            var id1 = new Token(Id, "asfsa");
            var colon = new Token(Colon, ":");
            var id2 = new Token(Id, "fasfas");
            var tokens = new[] { let, id1, colon, id2 };
            var node = rule.Read(new BufferedEnumerable<Token>(tokens));

            node.Should().BeOfType<NodeList>();
            var nodeList = (NodeList) node;
            var list = nodeList.Nodes.Select((n, i) => (n, t: tokens[i])).ToList();
            list.ForEach(p => p.n.Should().BeOfType<TokenNode>());
            list.ForEach(p => ((TokenNode) p.n).Token.Should().Be(p.t));
        }

        [Test]
        public void RecursiveRule()
        {
            CompositeRule tailRule = null;

            CompositeRule rrule = null;
            rrule = Rule.Named("testRule")
                .With(_ => Chain.StartWith(_)
                    .Then(Token(Colon))
                    .Then(Token(Id))
                    .Then(Token(Dot))
                    .Then(Token(Id))
                    .CollectTailBy(nodes => new NodeList(new[] { nodes[1], nodes[3] })))
                .With(Chain.StartWith(Token(Id))
                    .Then(Token(Dot))
                    .Then(Token(Id))
                    .CollectBy(nodes => new NodeList(new[] { nodes[0], nodes[2] })));

            var mainRule = CompositeRule.FixLeftRecursion(rrule, nodes => new NodeList(nodes));

            var id1 = new Token(Id, "a");
            var id2 = new Token(Id, "b");
            var id3 = new Token(Id, "c");
            var id4 = new Token(Id, "d");
            var colon = new Token(Colon, ":");
            var dot = new Token(Dot, ".");
            var tokens = new[] { id1, dot, id2, colon, id3, dot, id4 };
            var node = mainRule.Read(new BufferedEnumerable<Token>(tokens));

            node.Should().BeOfType<NodeList>();
            var nodeList = (NodeList) node;
            var list = nodeList.Nodes.Select((n, i) => (n, t: tokens[i])).ToList();
            list.ForEach(p => p.n.Should().BeOfType<NodeList>());
            list.ForEach(p =>
                ((NodeList) p.n).Nodes.Select(n => n.Should().BeOfType<TokenNode>()).Count().Should().Be(2));
        }
        [Test]
        public void MultipleRecursiveRule()
        {
            CompositeRule tailRule = null;

            CompositeRule rrule = null;
            rrule = Rule.Named("testRule")
                .With(_ => Chain.StartWith(_)
                    .Then(Token(Colon))
                    .Then(Token(Id))
                    .Then(Token(Dot))
                    .Then(Token(Id))
                    .CollectTailBy(nodes => new NodeList(new[] { nodes[1], nodes[3] })))
                .With(_ => Chain.StartWith(_)
                    .Then(Token(Colon))
                    .Then(Token(Id))
                    .Then(Token(Minus))
                    .Then(Token(Id))
                    .CollectTailBy(nodes => new NodeList(new[] { nodes[1], nodes[3] })))
                .With(Chain.StartWith(Token(Id))
                    .Then(Token(Dot))
                    .Then(Token(Id))
                    .CollectBy(nodes => new NodeList(new[] { nodes[0], nodes[2] })))
                .With(Chain.StartWith(Token(Id))
                    .Then(Token(Plus))
                    .Then(Token(Id))
                    .CollectBy(nodes => new NodeList(new[] { nodes[0], nodes[2] })));

            var mainRule = CompositeRule.FixLeftRecursion(rrule, nodes => new NodeList(nodes));

            var id1 = new Token(Id, "a");
            var id2 = new Token(Id, "b");
            var id3 = new Token(Id, "c");
            var id4 = new Token(Id, "d");
            var colon = new Token(Colon, ":");
            var dot = new Token(Dot, ".");
            var plus = new Token(Plus, "+");
            var minus = new Token(Minus, "-");
            var tokens = new[] { id1, plus, id2, colon, id3, minus, id4, colon, id1, dot, id2 };
            var node = mainRule.Read(new BufferedEnumerable<Token>(tokens));

            node.Should().BeOfType<NodeList>();
            var nodeList = (NodeList) node;
            var list = nodeList.Nodes.Select((n, i) => (n, t: tokens[i])).ToList();
            list.ForEach(p => p.n.Should().BeOfType<NodeList>());
            list.ForEach(p =>
                ((NodeList) p.n).Nodes.Select(n => n.Should().BeOfType<TokenNode>()).Count().Should().Be(2));
        }
    }
}