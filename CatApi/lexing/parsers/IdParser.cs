using CatApi.lexing.tokens;

namespace CatApi.lexing.parsers
{
    public class IdParser : IParser
    {
        public static bool IsCharacterAllowed(char character)
        {
            return char.IsLetter(character) || char.IsDigit(character) || character == '_';
        }

        public (IToken token, int length)? Parse(in string text)
        {
            if (!CanStartWith(text[0]))
                return null;
            for (int i = 1; i < text.Length; i++)
            {
                if (!IsCharacterAllowed(text[i]))
                {
                    var token = new Token(TokenTypes.Id, text[0..i]);
                    return (token, i);
                }
            }

            return (new Token(TokenTypes.Id, text), text.Length);
        }

        public bool CanStartWith(char character)
        {
            return char.IsLetter(character) || character == '_';
        }
    }
}