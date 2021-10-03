using System;
using CatApi.structures;

namespace CatImplementations.interpreting.exceptions
{
    public class CatVariableRedeclarationException : Exception
    {
        public CatVariableRedeclarationException(IScope scope, string variableName) : base($"Variable {variableName} is already defined in {scope.GetScopeName()}")
        {
            
        }
    }
}