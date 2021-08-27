using System;

namespace Cat.data.exceptions
{
    public class CatSecurityException : Exception
    {
        public CatSecurityException(string message) : base(message)
        {
        }
    }
}