using System.Collections.Generic;
using System.Linq;
using CatApi.lexing;
using CatApi.logger;
using CatImplementations.lexing.parsers;
using CatImplementations.lexing.tokens;
using CatImplementations.logging;

namespace CatImplementations.lexing
{
    public class Lexer : ILexer
    {
        private const string LexerSource = "Lexer";
        private readonly ILogger _logger;

        private List<IParser> _parsers = TokenTypes.SingleTokenTypes.Value
            .Select(t => new SimpleTokenParser(t))
            .Concat(TokenTypes.KeywordTokenTypes.Value
                .Select<KeyWordTokenType, IParser>(t => new KeywordTokenParser(t)))
            .Concat(new List<IParser>
            {
                new OperatorTokenParser(TokenTypes.OperatorTokenTypes.Value),
                new StringParser(),
                new NumberParser(),
                new BoolParser(),
                new IdParser()
            })
            .ToList();

        public Lexer(ILogger logger)
        {
            _logger = logger;
        }

        public IEnumerable<IToken> ParseCode(string sourceFile, string[] codeFile)
        {
            var tokens = new List<IToken>();
            var lastCodeLength = int.MaxValue;

            for (var codeLineNo = 0; codeLineNo < codeFile.Length; codeLineNo++)
            {
                var code = codeFile[codeLineNo];
                var codeCharacter = 0;

                while (code.Length > 0)
                {
                    while (code.Length < lastCodeLength && code.Length > 0)
                    {
                        lastCodeLength = code.Length;

                        var parsers = _parsers.Where(parser => parser.CanStartWith(code[0])).ToList();
                        foreach (var parser in parsers)
                        {
                            var result = parser.Parse(in code);
                            if (!result.HasValue) continue;

                            var (token, length) = result!.Value;
                            token.FillSource(sourceFile, codeLineNo, codeCharacter, codeCharacter + length);
                            tokens.Add(token);
                            codeCharacter += length;
                            code = code.Substring(length).Trim();
                            if (code.Length < lastCodeLength)
                                break;
                        }
                    }

                    if (code.Length > 0)
                    {
                        _logger.Log(LogLevel.Error, LexerSource,
                            $"Unexpected character \"{code[0]}\" at {sourceFile}:{codeLineNo + 1}:{codeCharacter + 1}");
                        code = code.Substring(1);
                        codeCharacter++;
                    }
                }
            }

            return tokens;
        }
    }
}