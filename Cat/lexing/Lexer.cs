using System;
using System.Collections.Generic;
using System.Linq;
using Cat.ast;
using Cat.lexing.parsers;

namespace Cat.lexing
{
    public interface ILexer
    {
        public IEnumerable<Token> ParseCode(string code);
    }

    public class Lexer : ILexer
    {
        private List<IParser> _parsers = new() {new StringParser(), new NumberParser()};

        public IEnumerable<Token> ParseCode(string code)
        {
            var tokens = new List<Token>();
            var lastCodeLength = int.MaxValue;

            void Consumer(Token token, int length)
            {
                tokens.Add(token);
                code = code.Substring(length).Trim();
            }

            while (code.Length < lastCodeLength && code.Length > 0)
            {
                lastCodeLength = code.Length;

                foreach (var parser in _parsers.Where(parser => parser.CanStartWith(code[0])))
                {
                    parser.Parse(in code, Consumer);
                }
            }

            return tokens;
        }
    }
}