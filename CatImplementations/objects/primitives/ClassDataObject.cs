using CatApi.types;

namespace CatImplementations.objects.primitives
{
    public class ClassDataObject : DataObject
    {
        public IDataType EnclosedType { get; }

        public ClassDataObject(IDataType type, IDataType enclosedType) : base(type)
        {
            EnclosedType = enclosedType;
        }
    }
}