using System;
using Cat.lexing.tokens;

namespace Cat.lexing
{
    public interface IParser
    {
        public void Parse(in string text, Action<Token, int> tokenConsumer);

        public bool CanStartWith(char character);
    }
}