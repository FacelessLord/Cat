using Cat.data.exceptions;

namespace Cat.interpret.exceptions
{
    public class CatVariableTypeUnknownException : CatTypeException
    {
        public CatVariableTypeUnknownException() : base("Variable can't be defined without type and value")
        {
        }
    }
}