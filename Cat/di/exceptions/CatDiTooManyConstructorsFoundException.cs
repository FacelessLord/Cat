using System;

namespace Cat.di.exceptions
{
    public class CatDiTooManyConstructorsFoundException : CatDiException
    {
        public CatDiTooManyConstructorsFoundException(Type t) : base($"Found too many constructors for type {t}")
        {
        }
    }
}