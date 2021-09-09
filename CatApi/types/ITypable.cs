namespace CatApi.types
{
    public interface ITypable
    {
        public IDataType Type { get; }

        public void AssignType(IDataType type);
    }
}