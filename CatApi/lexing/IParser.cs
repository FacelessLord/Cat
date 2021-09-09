namespace CatApi.lexing
{
    public interface IParser
    {
        public (IToken token, int length)? Parse(in string text);

        public bool CanStartWith(char character);
    }
}