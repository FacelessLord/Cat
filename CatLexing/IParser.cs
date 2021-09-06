using System;
using CatLexing.tokens;

namespace CatLexing
{
    public interface IParser
    {
        public (Token token, int length)? Parse(in string text);

        public bool CanStartWith(char character);
    }
}