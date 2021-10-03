using System;

namespace CatImplementations.interpreting.exceptions
{
    public class CatVariableDoesntExistException : Exception
    {
        public CatVariableDoesntExistException(string variableName) : base($"Variable {variableName} does not exist")
        {
            
        }
    }
}