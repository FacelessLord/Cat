using System;
using CatApi.types;
using CatApi.types.dataTypes;

namespace CatApi.exceptions
{
    public class CatPropertyUndefinedException : Exception
    {
        public CatPropertyUndefinedException(string property, IDataType owner) : base($"Property \"{property}\" is not defined on type {owner.FullName}")
        {
            
        }
    }
}