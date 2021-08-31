using System.Collections.Generic;
using Cat.data.types.api;
using Cat.interpret.exceptions;

namespace Cat.interpret
{
    public class TypingsStorage : ITypingsStorage
    {
        public Dictionary<string, IDataType> typings = new ();
        
        public void SetVariableType(string variableName, IDataType type)
        {
            typings[variableName] = type;
        }

        public IDataType GetVariableType(string variableName)
        {
            return typings[variableName];
        }

        public bool HasVariable(string variableName)
        {
            return typings.ContainsKey(variableName);
        }
    }
}