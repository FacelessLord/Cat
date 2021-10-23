using CatApi.types;
using CatApi.types.dataTypes;

namespace CatApi.exceptions
{
    public class CatTypeRedeclarationException : CatTypeException
    {
        public CatTypeRedeclarationException(IDataType cachedDataType, IDataType createdDataType) :
            base($"Type {cachedDataType} cannot be redeclared by {createdDataType}")
        {
        }
    }
}