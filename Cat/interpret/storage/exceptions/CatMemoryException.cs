using System;

namespace Cat.interpret.storage.exceptions
{
    public class CatMemoryException : Exception
    {
        public CatMemoryException(string message) : base(message)
        {
        }
    }
}