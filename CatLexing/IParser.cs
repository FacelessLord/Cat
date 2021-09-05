using System;
using CatLexing.tokens;

namespace CatLexing
{
    public interface IParser
    {
        public void Parse(in string text, Action<Token, int> tokenConsumer);

        public bool CanStartWith(char character);
    }
}