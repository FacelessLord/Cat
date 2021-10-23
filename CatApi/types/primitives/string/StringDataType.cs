using CatApi.objects;
using CatApi.types.containers;
using CatApi.types.primitives.@object;

namespace CatApi.types.primitives.@string
{
    public class StringDataType : ObjectDataType
    {
        private const string TypeName = "String";
        private const string TypeFullName = TypesPaths.System + "." + TypeName;

        public StringDataType()
        {
            FullName = TypeFullName;
        }

        public override void PopulateObject(IDataObject dataObject, params IDataObject[] args)
        {
            base.PopulateObject(dataObject);
            //todo add string props
        }
    }
}