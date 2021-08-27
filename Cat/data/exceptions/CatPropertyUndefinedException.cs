using System;
using Cat.data.properties.api;

namespace Cat.data.exceptions
{
    public class CatPropertyUndefinedException : Exception
    {
        public CatPropertyUndefinedException(IDataProperty property) : base($"Property {property.Name} is not defined on type {property.DeclaringType}")
        {
            
        }
    }
}