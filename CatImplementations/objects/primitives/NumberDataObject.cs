using CatApi.types;
using CatImplementations.objects.wrappers;

namespace CatImplementations.objects.primitives
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