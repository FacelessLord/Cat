using CatApi.objects;
using CatImplementations.objects.primitives;
using CatImplementations.objects.wrappers;
using CatImplementations.typings.primitives.@object;

namespace CatImplementations.typings.primitives.number
{
    public class NumberType : ObjectType
    {
        private const string TypeName = "Number";
        private const string TypeFullName = TypesPaths.System + TypeName;

        public NumberType()
        {
            Name = TypeName;
            FullName = TypeFullName;
        }

        public override void PopulateObject(IDataObject dataObject)
        {
            base.PopulateObject(dataObject);
        }

        public override IDataObject NewInstance(params object[] args)
        {
            return new NumberDataObject(this, NumberWrapper.FromObject(args[0]));
        }
    }
}