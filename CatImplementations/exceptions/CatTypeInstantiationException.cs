using System.Collections.Generic;
using CatApi.types;

namespace CatImplementations.exceptions
{
    public class CatTypeInstantiationException : CatTypeException
    {
        public CatTypeInstantiationException(IDataType type, IEnumerable<string> missedPropertys) :
            base($"Type {type.FullName} requires {string.Join(", ", missedPropertys)} properties to be implemented")
        {
        }
    }
}