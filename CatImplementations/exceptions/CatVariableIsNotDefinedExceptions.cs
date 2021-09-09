using System;

namespace CatImplementations.structures.exceptions
{
    public class CatVariableIsNotDefinedExceptions : Exception
    {
        public CatVariableIsNotDefinedExceptions(Scope scope, string variableName)
            : base($"Variable {variableName} is not defined or visible from scope {scope.GetScopeName()}")
        {
        }
    }
}