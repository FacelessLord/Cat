using Cat.data.types.api;

namespace Cat.interpret
{
    public interface ITypingsStorage
    {
        public void SetVariableType(string variableName, IDataType type);

        public IDataType GetVariableType(string variableName);
    }
}