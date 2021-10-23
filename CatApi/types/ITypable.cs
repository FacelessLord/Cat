using CatApi.types.dataTypes;

namespace CatApi.types
{
    public interface ITypable
    {
        public IDataType DataType { get; }

        public void AssignType(IDataType typeBox);
    }
}