﻿using System.Collections.Generic;
using System.Linq;
using CatLexing.parsers;
using CatLexing.tokens;

namespace CatLexing
{
    public interface ILexer
    {
        public IEnumerable<Token> ParseCode(string code);
    }

    public class Lexer : ILexer
    {
        private List<IParser> _parsers = TokenTypes.SingleTokenTypes.Value
            .Select(t => new SimpleTokenParser(t))
            .Concat(new List<IParser>
            {
                new OperatorTokenParser(TokenTypes.OperatorTokenTypes.Value),
                new StringParser(),
                new NumberParser(),
                new BoolParser(),
                new IdParser()
            })
            .ToList();

        public IEnumerable<Token> ParseCode(string code)
        {
            var tokens = new List<Token>();
            var lastCodeLength = int.MaxValue;

            void Consumer(Token token, int length)
            {
                tokens.Add(token);
                code = code.Substring(length).Trim();
            }

            while (code.Length > 0)
            {
                while (code.Length < lastCodeLength && code.Length > 0)
                {
                    lastCodeLength = code.Length;

                    var parsers = _parsers.Where(parser => parser.CanStartWith(code[0])).ToList();
                    foreach (var parser in parsers)
                    {
                        parser.Parse(in code, Consumer);
                        if(code.Length < lastCodeLength)
                            break;
                    }
                }

                if (code.Length > 0)
                    code = code.Substring(1);
            }

            return tokens;
        }
    }
}