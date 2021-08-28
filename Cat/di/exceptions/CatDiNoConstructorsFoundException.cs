using System;

namespace Cat.di.exceptions
{
    public class CatDiNoConstructorsFoundException : CatDiException
    {
        public CatDiNoConstructorsFoundException(Type t) : base($"Couldn't find constructor for type {t}")
        {
        }
    }
}