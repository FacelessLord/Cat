using Cat.data.types.api;

namespace Cat.data.exceptions
{
    public class CatTypeRedeclarationException : CatTypeException
    {
        public CatTypeRedeclarationException(IDataType cachedType, IDataType createdType) :
            base($"Type {cachedType} cannot be redeclared by {createdType}")
        {
        }
    }
}