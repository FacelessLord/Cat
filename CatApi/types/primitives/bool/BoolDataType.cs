using CatApi.objects;
using CatApi.types.containers;
using CatApi.types.primitives.@object;

namespace CatApi.types.primitives.@bool
{
    public class BoolDataType : ObjectDataType
    {
        private const string TypeName = "Bool";
        private const string TypeFullName = TypesPaths.System + TypeName;

        public BoolDataType()
        {
            FullName = TypeFullName;
        }

        public override void PopulateObject(IDataObject dataObject, params IDataObject[] args)
        {
            base.PopulateObject(dataObject);
        }
    }
}