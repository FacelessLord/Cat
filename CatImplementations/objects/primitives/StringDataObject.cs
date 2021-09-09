using CatApi.types;

namespace CatImplementations.objects.primitives
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