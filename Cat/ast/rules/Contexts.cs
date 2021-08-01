using Cat.lexing.tokens;

namespace Cat.ast.rules
{
    public static class Contexts
    {
        public static FunctionBodyRule FunctionBody => new();
        public static PipelineRule Pipeline => new();
        public static PipelineRuleT PipelineT => new();
        public static PipelineRuleF PipelineF => new();
        public static ExpressionRule Expression => new();
        public static LiteralRule Literal => new();

        //todo add caching
        public static TokenRule Token(ITokenType tokenType) => new (tokenType);
    }
}