namespace CatApi.lexing
{
    public interface IToken : IHaveSource
    {
        public ITokenType Type { get; }
        public string Value { get; }
    }
}