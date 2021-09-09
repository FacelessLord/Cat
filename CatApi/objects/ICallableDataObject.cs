namespace CatApi.objects
{
    public interface ICallableDataObject
    {
        public IDataObject Call(IDataObject[] args);
    }
}