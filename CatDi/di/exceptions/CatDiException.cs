using System;

namespace CatDi.di.exceptions
{
    public class CatDiException : Exception
    {
        public CatDiException(string message) : base(message)
        {
        }
    }
}