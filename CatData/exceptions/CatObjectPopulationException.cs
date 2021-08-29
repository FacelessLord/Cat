using System;
using Cat.data.types.api;

namespace Cat.data.exceptions
{
    public class CatObjectPopulationException : Exception
    {
        public CatObjectPopulationException(IDataType type) : base($"Type {type.FullName} can't populate object")
        {
        }
    }
}