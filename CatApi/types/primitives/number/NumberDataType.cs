using CatApi.objects;
using CatApi.types.containers;
using CatApi.types.primitives.@object;

namespace CatApi.types.primitives.number
{
    public class NumberDataType : ObjectDataType
    {
        private const string TypeName = "Number";
        private const string TypeFullName = TypesPaths.System + TypeName;

        public NumberDataType()
        {
            FullName = TypeFullName;
        }

        public override void PopulateObject(IDataObject dataObject, params IDataObject[] args)
        {
            base.PopulateObject(dataObject);
        }
    }
}