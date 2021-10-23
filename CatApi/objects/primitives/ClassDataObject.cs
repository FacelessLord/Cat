using CatApi.types;
using CatApi.types.dataTypes;
using CatApi.types.primitives;

namespace CatApi.objects.primitives
{
    public class ClassDataObject : DataObject
    {
        public IDataType EnclosedDataType { get; }

        public ClassDataObject(IDataType enclosedDataType) : base(Primitives.Object)
        {
            EnclosedDataType = enclosedDataType;
        }
    }
}