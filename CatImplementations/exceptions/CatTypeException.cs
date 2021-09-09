using System;

namespace CatImplementations.exceptions
{
    public class CatTypeException : Exception
    {
        public CatTypeException(string message) : base(message)
        {
            
        }
    }
}