namespace CatApi.types
{
    public interface ICallableDataType : IDataType
    {
        public IDataType[] SourceTypes { get; }
        public IDataType TargetType { get; }
    }
}