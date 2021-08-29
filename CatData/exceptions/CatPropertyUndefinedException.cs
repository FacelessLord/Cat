using System;
using Cat.data.properties.api;
using Cat.data.types.api;

namespace Cat.data.exceptions
{
    public class CatPropertyUndefinedException : Exception
    {
        public CatPropertyUndefinedException(IDataProperty property, IDataType owner) : base($"Property {property.Name} is not defined on type {owner.Name}")
        {
            
        }
    }
}