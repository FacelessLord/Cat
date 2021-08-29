using Cat.data.objects.api;
using Cat.data.objects.primitives;
using Cat.data.objects.wrappers;
using Cat.data.types.primitives.@object;

namespace Cat.data.types.primitives.number
{
    public class NumberType : ObjectType
    {
        private const string TypeName = "Number";
        private const string TypeFullName = TypesPaths.System + TypeName;

        public NumberType(TypeStorage types) : base(types)
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