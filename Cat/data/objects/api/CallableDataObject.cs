using Cat.data.objects.primitives;
using Cat.data.types.api;

namespace Cat.data.objects.api
{
    public abstract class CallableDataObject : DataObject, ICallableDataObject
    {
        public CallableDataObject(IDataType type) : base(type)
        {
        }

        public abstract IDataObject Call(IDataObject[] args);
    }
}