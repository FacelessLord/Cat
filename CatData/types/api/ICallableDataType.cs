namespace Cat.data.types.api
{
    public interface ICallableDataType : IDataType
    {
        public IDataType[] SourceTypes { get; }
        public IDataType TargetType { get; }
    }
}