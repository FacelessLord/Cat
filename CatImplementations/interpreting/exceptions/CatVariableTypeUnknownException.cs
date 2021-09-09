using CatImplementations.exceptions;

namespace CatImplementations.interpreting.exceptions
{
    public class CatVariableTypeUnknownException : CatTypeException
    {
        public CatVariableTypeUnknownException() : base("Variable can't be defined without type and value")
        {
        }
    }
}