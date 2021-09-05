using System;
using CatLexing.tokens;

namespace CatLexing.parsers
{
    public class SimpleTokenParser : IParser
    {
        public SimpleTokenType Type { get; }

        public SimpleTokenParser(SimpleTokenType type)
        {
            Type = type;
        }

        public void Parse(in string text, Action<Token, int> tokenConsumer)
        {
            if (text.StartsWith(Type.Terminal))
            {
                tokenConsumer(new Token(Type, Type.Terminal), Type.Terminal.Length);
            }
        }

        public bool CanStartWith(char character)
        {
            return Type.Terminal[0] == character;
        }
    }
}