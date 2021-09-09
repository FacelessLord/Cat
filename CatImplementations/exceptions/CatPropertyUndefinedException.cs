using System;
using CatApi.structures;
using CatApi.types;

namespace CatImplementations.exceptions
{
    public class CatPropertyUndefinedException : Exception
    {
        public CatPropertyUndefinedException(IDataProperty property, IDataType owner) : base($"Property {property.Name} is not defined on type {owner.Name}")
        {
            
        }
    }
}