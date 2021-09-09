using CatApi.lexing;
using CatImplementations.lexing.tokens;

namespace CatImplementations.lexing.parsers
{
    public class KeywordTokenParser : IParser
    {
        public KeyWordTokenType Type { get; }

        public KeywordTokenParser(KeyWordTokenType type)
        {
            Type = type;
        }

        public (IToken token, int length)? Parse(in string text)
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