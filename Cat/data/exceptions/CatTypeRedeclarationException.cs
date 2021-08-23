using System;
using Cat.data.types;

namespace Cat.data.exceptions
{
    public class CatTypeRedeclarationException : Exception
    {
        public CatTypeRedeclarationException(DataType cachedType, DataType createdType) :
            base($"Type {cachedType} cannot be redeclared by {createdType}")
        {
        }
    }
}