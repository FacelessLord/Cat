using CatApi.types;

namespace CatImplementations.exceptions
{
    public class CatTypeRedeclarationException : CatTypeException
    {
        public CatTypeRedeclarationException(IDataType cachedType, IDataType createdType) :
            base($"Type {cachedType} cannot be redeclared by {createdType}")
        {
        }
    }
}