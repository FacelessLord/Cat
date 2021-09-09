using CatImplementations.objects.primitives;
using CatImplementations.typings.primitives.@object;

namespace CatImplementations.typings.primitives.@bool
{
    public class BoolType : ObjectType
    {
        private const string TypeName = "Bool";
        private const string TypeFullName = TypesPaths.System + TypeName;

        public BoolType()
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
            return new BoolDataObject(this, (bool) args[0]);
        }
    }
}