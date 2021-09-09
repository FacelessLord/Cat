using CatApi.objects;
using CatApi.types;

namespace CatImplementations.objects.primitives
{
    public abstract class CallableDataObject : DataObject, ICallableDataObject
    {
        public CallableDataObject(IDataType type) : base(type)
        {
        }

        public abstract IDataObject Call(IDataObject[] args);
    }
}