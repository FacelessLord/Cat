using System;

namespace CatImplementations.exceptions
{
    public class CatSecurityException : Exception
    {
        public CatSecurityException(string message) : base(message)
        {
        }
    }
}