namespace CatApi.exceptions
{
    public class CatVariableTypeUnknownException : CatTypeException
    {
        public CatVariableTypeUnknownException() : base("Variable can't be defined without type and value")
        {
        }
    }
}