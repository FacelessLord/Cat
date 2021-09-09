using CatApi.lexing;
using CatCollections;

namespace CatApi.interpreting
{
    public interface IRule
    {
        public INode Read(BufferedEnumerable<IToken> tokens);
    }
}