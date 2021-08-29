using Cat.data.objects.wrappers;
using Cat.data.types.api;

namespace Cat.data.objects.primitives
{
    public class NumberDataObject : DataObject
    {
        public NumberWrapper Value { get; }

        public NumberDataObject(IDataType type, NumberWrapper value) : base(type)
        {
            Value = value;
        }
    }
}