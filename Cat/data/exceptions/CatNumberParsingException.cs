using System;
using Cat.data.properties;
using Cat.data.properties.api;
using Cat.data.types.api;

namespace Cat.data.exceptions
{
    public class CatNumberParsingException : Exception
    {
        public CatNumberParsingException(object src) :
            base($"Cannot cast {src} to number")
        {
        }
    }
}