using System;
using CatLexing.tokens;

namespace CatLexing.parsers
{
    public class IdParser : IParser
    {
        public static bool IsCharacterAllowed(char character)
        {
            return char.IsLetter(character) || char.IsDigit(character) || character == '_';
        }

        public void Parse(in string text, Action<Token, int> tokenConsumer)
        {
            if (!CanStartWith(text[0]))
                return;
            for (int i = 1; i < text.Length; i++)
            {
                if (!IsCharacterAllowed(text[i]))
                {
                    var token = new Token(TokenTypes.Id, text[0..i]);
                    tokenConsumer(token, i);
                    return;
                }
            }

            tokenConsumer(new Token(TokenTypes.Id, text), text.Length);
        }

        public bool CanStartWith(char character)
        {
            return char.IsLetter(character) || character == '_';
        }
    }
}