using CatImplementations.objects.primitives;
using CatImplementations.typings.primitives.@object;

namespace CatImplementations.typings.primitives.@string
{
    public class StringType : ObjectType
    {
        private const string TypeName = "String";
        private const string TypeFullName = TypesPaths.System + "." + TypeName;

        public StringType()
        {
            Name = TypeName;
            FullName = TypeFullName;
        }

        public override void PopulateObject(IDataObject dataObject)
        {
            base.PopulateObject(dataObject);
            //todo add string props
        }

        public override IDataObject NewInstance(params object[] args)
        {
            return new StringDataObject(this, args[0] as string);
        }
    }
}