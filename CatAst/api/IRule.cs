using Cat.ast.api;
using Cat.lexing.tokens;

namespace Cat.ast.new_ast
{
    public interface IRule
    {
        public INode Read(BufferedEnumerable<Token> tokens);
    }
}