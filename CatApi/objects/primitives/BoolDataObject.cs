using CatApi.types.primitives;

namespace CatApi.objects.primitives
{
    public class BoolDataObject : DataObject
    {
        public bool Value { get; }

        public BoolDataObject(bool value) : base(Primitives.Object)
        {
            Value = value;
        }
    }
}