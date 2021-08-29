using System;

namespace CatDi.di.exceptions
{
    public class CatDiCyclicImportException : CatDiException
    {
        public CatDiCyclicImportException(Type a) : base(
            $"Found Cyclic import for type {a.FullName}")
        {
        }
    }
}