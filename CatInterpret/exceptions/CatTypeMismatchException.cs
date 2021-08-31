using Cat.data.exceptions;
using Cat.data.types.api;

namespace Cat.interpret.exceptions
{
    public class CatTypeMismatchException : CatTypeException
    {
        public CatTypeMismatchException(IDataType from, IDataType to) : base(
            $"Type {from.FullName} is unassignable to type {to.FullName}")
        {
        }
    }
}