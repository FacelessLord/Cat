namespace Cat.data.objects.api
{
    public interface ICallableDataObject
    {
        public IDataObject Call(IDataObject[] args);
    }
}