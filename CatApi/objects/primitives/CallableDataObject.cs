using CatApi.types;

namespace CatApi.objects.primitives
{
    public abstract class CallableDataObject : DataObject, ICallableDataObject
    {
        public CallableDataObject(TypeBox typeBox) : base(typeBox)
        {
        }

        public abstract IDataObject Call(IDataObject[] args);
    }
}