using Cat.data.types.api;

namespace Cat.interpret
{
    public interface ITyped
    {
        public IDataType Type { get; }
    }
}