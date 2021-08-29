using System;

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