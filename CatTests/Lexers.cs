using System;
using CatLexing.parsers;
using CatLexing.tokens;
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
        public Lazy<IdParser> idParser = new(() => new IdParser());

        public Lazy<SimpleTokenParser> simpleTokenParser(SimpleTokenType token) =>
            new(() => new SimpleTokenParser(token));

        public static Action<Token, int> DebugConsumer =
            ((token, i) => Console.Error.WriteLine($"{token.Type}, {token.Value}, {i}"));

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
            var expected = new Token(TokenTypes.Number, value);

            var result = parser.Parse(value);

            result.HasValue.Should().BeTrue();

            var (token, length) = result!.Value;

            token.Should().Be(expected);
            length.Should().Be(value.Length);
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
            var expectedToken = new Token(TokenTypes.Number, value);

            var result = parser.Parse(value);

            (!result.HasValue || !result!.Value.token.Equals(expectedToken)).Should().BeTrue();
        }

        [Test]
        [TestCase("''", "", TestName = "empty quoted string")]
        [TestCase("'asf'", "asf", TestName = "quoted string with letters")]
        [TestCase("'asd dads'", "asd dads", TestName = "quoted string with spaces")]
        [TestCase("'asd 124'", "asd 124", TestName = "quoted string with numbers")]
        [TestCase("'\\'asd 124'", "'asd 124", TestName = "quoted string with escaped apostrophes")]
        [TestCase("'\\'asd\\' 124'", "'asd' 124", TestName = "quoted string with escaped apostrophes*2")]
        public void StringParser_ParsesQuotedStrings(string value, string expected)
        {
            var parser = stringParser.Value;
            var expectedToken = new Token(TokenTypes.String, expected);

            var result = parser.Parse(value);

            result.HasValue.Should().BeTrue();

            var (token, length) = result!.Value;

            token.Should().Be(expectedToken);
            length.Should().Be(value.Length);
        }

        [Test]
        [TestCase("\"\"", "", TestName = "empty quoted string")]
        [TestCase("\"asf\"", "asf", TestName = "quoted string with letters")]
        [TestCase("\"asd dads\"", "asd dads", TestName = "quoted string with spaces")]
        [TestCase("\"asd 124\"", "asd 124", TestName = "quoted string with numbers")]
        [TestCase("\"\\\"asd 124\"", "\"asd 124", TestName = "quoted string with escaped quotes")]
        [TestCase("\"\\\"asd\\\" 124\"", "\"asd\" 124", TestName = "quoted string with escaped quotes*2")]
        public void StringParser_ParsesDoubleQuotedStrings(string value, string expected)
        {
            var parser = stringParser.Value;
            var expectedToken = new Token(TokenTypes.String, expected);

            var result = parser.Parse(value);

            result.HasValue.Should().BeTrue();

            var (token, length) = result!.Value;

            token.Should().Be(expectedToken);
            length.Should().Be(value.Length);
        }

        [Test]
        [TestCase("aaaaaa", TestName = "one word")]
        [TestCase("aaaaaa123", TestName = "one word with digits")]
        [TestCase("_affsas", TestName = "start with lodash")]
        public void IdParser_ParsesIds(string value)
        {
            var parser = idParser.Value;
            var expectedToken = new Token(TokenTypes.Id, value);

            var result = parser.Parse(value);

            result.HasValue.Should().BeTrue();

            var (token, length) = result!.Value;

            token.Should().Be(expectedToken);
            length.Should().Be(value.Length);
        }

        [Test]
        [TestCase(" aaaaaa", TestName = "start with space")]
        [TestCase("123aaaaaa", TestName = "one word starting with digits")]
        [TestCase(" affsas", TestName = "start with space")]
        [TestCase("affsas:", TestName = "with colon")]
        public void IdParser_DoesntParseIds(string value)
        {
            var parser = idParser.Value;
            var expectedToken = new Token(TokenTypes.Id, value);

            var result = parser.Parse(value);

            (!result.HasValue || !result!.Value.token.Equals(expectedToken)).Should().BeTrue();
        }
    }
}