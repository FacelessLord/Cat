using System.Collections.Generic;
using CatApi.types;
using CatApi.types.dataTypes;

namespace CatApi.exceptions
{
    public class CatTypeInstantiationException : CatTypeException
    {
        public CatTypeInstantiationException(IDataType typeBox, IEnumerable<string> missedPropertys) :
            base($"Type {typeBox.FullName} requires {string.Join(", ", missedPropertys)} properties to be implemented")
        {
        }
    }
}