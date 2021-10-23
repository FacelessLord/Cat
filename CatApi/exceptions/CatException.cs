using System;

namespace CatApi.exceptions
{
    public class CatException : Exception
    {
        public CatException(string message) : base(message)
        {
        }
    }
}