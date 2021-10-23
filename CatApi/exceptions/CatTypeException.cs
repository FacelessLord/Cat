using System;

namespace CatApi.exceptions
{
    public class CatTypeException : Exception
    {
        public CatTypeException(string message) : base(message)
        {
            
        }
    }
}