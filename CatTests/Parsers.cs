using System;
using System.Linq;
using Cat.ast;
using Cat.ast.nodes;
using Cat.ast.rules;
using Cat.lexing;
using FluentAssertions;
using NUnit.Framework;

namespace CatTests
{
    [TestFixture]
    public class Parsers
    {
        private static Lazy<Lexer> lexer = new(() => new Lexer());
        private static Lazy<Parser> parser = new(() => new Parser());
        
        [Test]
        public void ParserParsesSimplePipeline()
        {
            var code = "\"1\" | 124 & '12'";
            var tokens = lexer.Value.ParseCode(code);
            var node = parser.Value.TryParse(Contexts.FunctionBody, Parser.OnError, tokens);

            node.Should().BeOfType<PipelineOrNode>();
            var pipelineOrNode = (PipelineOrNode)node;
            pipelineOrNode.A.Should().BeOfType<StringNode>();
            pipelineOrNode.B.Should().BeOfType<PipelineAndNode>();
            var pipelineAndNode = (PipelineAndNode)pipelineOrNode.B;
            pipelineAndNode.A.Should().BeOfType<NumberNode>();
            pipelineAndNode.B.Should().BeOfType<StringNode>();
        }
    }
}