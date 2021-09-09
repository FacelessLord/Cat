using System.Collections.Generic;

namespace CatApi.lexing
{
    public interface ILexer
    {
        public IEnumerable<IToken> ParseCode(string sourceFile, string[] code);
    }
}