using Cat.data.types.api;

namespace Cat.data.objects.primitives
{
    public class StringDataObject : DataObject
    {
        public string Value { get; }

        public StringDataObject(IDataType type, string value) : base(type)
        {
            Value = value;
        }
    }
}