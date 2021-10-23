using CatApi.objects.wrappers;
using CatApi.types;

namespace CatApi.objects.primitives
{
    public class NumberDataObject : DataObject
    {
        public NumberWrapper Value { get; }

        public NumberDataObject(TypeBox typeBox, NumberWrapper value) : base(typeBox)
        {
            Value = value;
        }
    }
}