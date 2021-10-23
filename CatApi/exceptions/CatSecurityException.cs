using System;

namespace CatApi.exceptions
{
    public class CatSecurityException : Exception
    {
        public CatSecurityException(string message) : base(message)
        {
        }
    }
}