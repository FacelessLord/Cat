using CatApi.lexing;
using CatApi.utils;

namespace CatApi.interpreting
{
    public interface IRule
    {
        public INode Read(BufferedEnumerable<IToken> tokens);
    }
}