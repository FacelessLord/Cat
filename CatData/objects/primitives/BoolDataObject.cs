using Cat.data.types.api;

namespace Cat.data.objects.primitives
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