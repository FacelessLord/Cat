using System.Collections.Generic;
using Cat.data.types.api;

namespace Cat.data.exceptions
{
    public class CatTypeInstantiationException : CatTypeException
    {
        public CatTypeInstantiationException(IDataType type, IEnumerable<string> missedPropertys) :
            base($"Type {type.FullName} requires {string.Join(", ", missedPropertys)} properties to be implemented")
        {
        }
    }
}