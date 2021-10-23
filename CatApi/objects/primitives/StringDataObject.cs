using CatApi.types.primitives;

namespace CatApi.objects.primitives
{
    public class StringDataObject : DataObject
    {
        public string Value { get; }

        public StringDataObject(string value) : base(Primitives.String)
        {
            Value = value;
        }
    }
}