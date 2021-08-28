using System;

namespace Cat.di.exceptions
{
    public class CatDiException : Exception
    {
        public CatDiException(string message) : base(message)
        {
        }
    }
}