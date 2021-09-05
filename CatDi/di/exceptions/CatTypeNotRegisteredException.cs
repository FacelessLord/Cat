using System;

namespace CatDi.di.exceptions
{
    public class CatTypeNotRegisteredException : CatDiException
    {
        public CatTypeNotRegisteredException(Type t) : base($"Type {t.FullName} is not registered in kernel")
        {
        }
    }
}