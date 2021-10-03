using CatApi.types;
using CatImplementations.exceptions;

namespace CatImplementations.interpreting.exceptions
{
    public class CatTypeMismatchException : CatTypeException
    {
        public CatTypeMismatchException(IDataType from, IDataType to) : base(
            $"Type {from.FullName} is unassignable to type {to.FullName}")
        {
        }
    }
}