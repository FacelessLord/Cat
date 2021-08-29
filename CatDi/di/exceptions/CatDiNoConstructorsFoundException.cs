using System;

namespace CatDi.di.exceptions
{
    public class CatDiNoConstructorsFoundException : CatDiException
    {
        public CatDiNoConstructorsFoundException(Type t) : base($"Couldn't find constructor for type {t}")
        {
        }
    }
}