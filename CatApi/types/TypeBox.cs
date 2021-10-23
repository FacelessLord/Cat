using CatApi.types.dataTypes;

namespace CatApi.types
{
    public class TypeBox
    {
        public TypeBox(string Id)
        {
            this.Id = Id;
        }

        public static TypeBox From(IDataType type)
        {
            return new TypeBox(type.FullName);
        }

        public string Id { get; init; }

        public void Deconstruct(out string Id)
        {
            Id = this.Id;
        }
    }
}