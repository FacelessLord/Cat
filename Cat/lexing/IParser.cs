using System;
using Cat.ast;

namespace Cat.lexing
{
    public interface IParser
    {
        public void Parse(in string text, Action<Token, int> tokenConsumer);

        public bool CanStartWith(char character);
    }
}