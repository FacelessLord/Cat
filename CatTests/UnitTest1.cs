using System;
using Cat.ast;
using Cat.lexing.parsers;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace CatTests
{
    [TestFixture]
    public class ParsersTests
    {
        public Lazy<NumberParser> numberParser = new(() => new NumberParser());
        public Lazy<StringParser> stringParser = new(() => new StringParser());

        [Test]
        [TestCase("124125125", TestName = "integers")]
        [TestCase(".124125125", TestName = "decimals less than one")]
        [TestCase("124.124125125", TestName = "decimals")]
        [TestCase("-124125125", TestName = "integers with minus")]
        [TestCase("-.124125125", TestName = "decimals less than one with minus")]
        [TestCase("-124.124125125", TestName = "decimals with minus")]
        public void NumberParser_ParsesCorrectly(string value)
        {
            var parser = numberParser.Value;
            var expected = new Token(TokenTypes.number, value);

            var Consumer = A.Fake<Action<Token, int>>();

            parser.Parse(value, Consumer);

            A.CallTo(Consumer)
                .WhenArgumentsMatch(f => f[0].Equals(expected) && f[1].Equals(value.Length))
                .MustHaveHappened(1, Times.Exactly);
        }
        [Test]
        [TestCase("12412 5125", TestName = "spaces inside")]
        [TestCase(". 124125125", TestName = "spaces after dot")]
        [TestCase("124-.124125125", TestName = "minus inside")]
        [TestCase("--124125125", TestName = "two minuses")]
        [TestCase("-.124125.125", TestName = "two dots")]
        [TestCase("- 124.124125125", TestName = "space after minus")]
        public void NumberParser_DoesntParse(string value)
        {
            var parser = numberParser.Value;
            var expected = new Token(TokenTypes.number, value);

            var Consumer = A.Fake<Action<Token, int>>();

            parser.Parse(value, Consumer);

            A.CallTo(Consumer)
                .WhenArgumentsMatch(f => f[0].Equals(expected) && f[1].Equals(value.Length))
                .MustHaveHappened(0, Times.Exactly);
        }
    }
}