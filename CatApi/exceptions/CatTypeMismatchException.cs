using CatApi.types;
using CatApi.types.dataTypes;

namespace CatApi.exceptions
{
    public class CatTypeMismatchException : CatTypeException
    {
        public CatTypeMismatchException(IDataType from, IDataType to) : base(
            $"Type {from.FullName} is unassignable to type {to.FullName}")
        {
        }
    }
}