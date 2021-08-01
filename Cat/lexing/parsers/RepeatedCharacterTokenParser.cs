using System;
using Cat.lexing.tokens;

namespace Cat.lexing.parsers
{
    public class RepeatedCharacterTokenParser : IParser
    {
        private readonly char _character;
        private readonly RepetitionVariantTokenType[] _tokenTypes;

        public RepeatedCharacterTokenParser(char character, params RepetitionVariantTokenType[] tokenTypes)
        {
            _character = character;
            _tokenTypes = tokenTypes;
        }

        private ITokenType GetTokenTypeByRepetitions(int repetitions)
        {
            return repetitions <= _tokenTypes.Length ? _tokenTypes[repetitions-1] : TokenTypes.Unknown;
        }

        public void Parse(in string text, Action<Token, int> tokenConsumer)
        {
            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == _character)
                    continue;
                tokenConsumer(new Token(GetTokenTypeByRepetitions(i), text[..i]), i);
            }
        }

        public bool CanStartWith(char character)
        {
            return character == _character;
        }
    }
}