using Cat.data.objects.api;
using Cat.data.objects.primitives;
using Cat.data.types.primitives.@object;

namespace Cat.data.types.primitives.@string
{
    public class StringType : ObjectType
    {
        private const string TypeName = "String";
        private const string TypeFullName = TypesPaths.System + "." + TypeName;

        public StringType(TypeStorage types) : base(types)
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