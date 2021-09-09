using CatApi.types;

namespace CatImplementations.objects.primitives
{
    public class BoolDataObject : DataObject
    {
        public bool Value { get; }

        public BoolDataObject(IDataType type, bool value) : base(type)
        {
            Value = value;
        }
    }
}