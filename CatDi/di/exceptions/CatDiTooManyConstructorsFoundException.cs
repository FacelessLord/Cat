using System;

namespace CatDi.di.exceptions
{
    public class CatDiTooManyConstructorsFoundException : CatDiException
    {
        public CatDiTooManyConstructorsFoundException(Type t) : base($"Found too many constructors for type {t}")
        {
        }
    }
}