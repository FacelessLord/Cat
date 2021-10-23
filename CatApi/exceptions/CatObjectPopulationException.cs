using System;
using CatApi.types;
using CatApi.types.dataTypes;

namespace CatApi.exceptions
{
    public class CatObjectPopulationException : Exception
    {
        public CatObjectPopulationException(IDataType typeBox) : base($"Type {typeBox.FullName} can't populate object")
        {
        }
    }
}