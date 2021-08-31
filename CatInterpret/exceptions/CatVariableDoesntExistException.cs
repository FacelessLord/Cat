using System;

namespace Cat.interpret.exceptions
{
    public class CatVariableDoesntExistException : Exception
    {
        public CatVariableDoesntExistException(string variableName) : base($"Variable {variableName} does not exist")
        {
            
        }
    }
}