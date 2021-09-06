using CatLexing.tokens;

namespace CatLexing.parsers
{
    public class KeywordTokenParser : IParser
    {
        public KeyWordTokenType Type { get; }

        public KeywordTokenParser(KeyWordTokenType type)
        {
            Type = type;
        }

        public (Token token, int length)? Parse(in string text)
        {
            if (text.StartsWith(Type.Terminal) &&
                (text.Length <= Type.Terminal.Length || !char.IsLetter(text[Type.Terminal.Length])))
            {
                return (new Token(Type, Type.Terminal), Type.Terminal.Length);
            }

            return null;
        }

        public bool CanStartWith(char character)
        {
            return Type.Terminal[0] == character;
        }
    }
}