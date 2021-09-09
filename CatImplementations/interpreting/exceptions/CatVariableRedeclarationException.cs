using System;

namespace CatImplementations.interpreting.exceptions
{
    public class CatVariableRedeclarationException : Exception
    {
        public CatVariableRedeclarationException(string variableName) : base($"Variable {variableName} is already defined")
        {
            
        }
    }
}