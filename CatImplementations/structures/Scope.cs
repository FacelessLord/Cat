using System.Collections.Generic;
using CatApi.structures;
using CatImplementations.interpreting.exceptions;
using CatImplementations.structures.exceptions;
using CatImplementations.structures.variables;

namespace CatImplementations.structures
{
    public class Scope : IScope
    {
        public string Name { get; }
        public IScope ParentScope { get; }

        private Dictionary<string, IVariable> Variables = new Dictionary<string, IVariable>();

        public Scope(string name, Scope parentScope = null)
        {
            Name = name;
            ParentScope = parentScope;
        }

        public IVariable GetVariable(string name)
        {
            if (Variables.ContainsKey(name))
            {
                return Variables[name];
            }

            if (ParentScope != null)
                return ParentScope.GetVariable(name);
            throw new CatVariableIsNotDefinedExceptions(this, name);
        }

        public void CreateVariable(IVariable variable)
        {
            if (!VariableExistsInCurrentScope(variable.Name))
                Variables[variable.Name] = variable;
            else
                throw new CatVariableRedeclarationException(this, variable.Name);
        }

        public bool VariableExists(string name)
        {
            return VariableExistsInCurrentScope(name) || (ParentScope != null && ParentScope.VariableExists(name));
        }

        public bool VariableExistsInCurrentScope(string name)
        {
            return Variables.ContainsKey(name);
        }

        public string GetScopeName()
        {
            return ParentScope != null ? ParentScope.GetScopeName() + "." + Name : Name;
        }
    }
}