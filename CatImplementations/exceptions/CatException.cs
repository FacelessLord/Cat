using System;

namespace CatImplementations.exceptions
{
    public class CatException : Exception
    {
        public CatException(string message) : base(message)
        {
        }
    }
}