using System;
using CatApi.structures;
using CatApi.types;

namespace CatImplementations.exceptions
{
    public class CatPropertyUndefinedException : Exception
    {
        public CatPropertyUndefinedException(string property, IDataType owner) : base($"Property \"{property}\" is not defined on type {owner.Name}")
        {
            
        }
    }
}