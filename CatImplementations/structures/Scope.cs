using System.Collections.Generic;
using CatImplementations.structures.exceptions;
using CatImplementations.structures.variables;

namespace CatImplementations.structures
{
    public class Scope
    {
        public string Name { get; }
        private Scope parentScope;

        private Dictionary<string, Variable> Variables = new Dictionary<string, Variable>();

        public Scope(string name, Scope parentScope = null)
        {
            Name = name;
            this.parentScope = parentScope;
        }

        public Variable FindVariable(string name)
        {
            if (Variables.ContainsKey(name))
            {
                return Variables[name];
            }

            if (parentScope != null)
                return parentScope.FindVariable(name);
            throw new CatVariableIsNotDefinedExceptions(this, name);
        }

        public string GetScopeName()
        {
            return parentScope.parentScope != null ? parentScope.GetScopeName() + "." + Name : Name;
        }
    }
}