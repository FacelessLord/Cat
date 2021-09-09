using System;
using CatApi.types;

namespace CatImplementations.exceptions
{
    public class CatObjectPopulationException : Exception
    {
        public CatObjectPopulationException(IDataType type) : base($"Type {type.FullName} can't populate object")
        {
        }
    }
}