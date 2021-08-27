using System.Linq;
using Cat.data.objects;
using Cat.data.objects.api;
using Cat.data.properties;
using Cat.data.types.primitives.@string;

namespace Cat.data.types.primitives.@object
{
    public class ToStringMethod : ObjectMethod
    {
        private readonly TypeStorage _types;

        public ToStringMethod(TypeStorage types) : base(
            types.Function(types[Primitives.Object], types[Primitives.String]),
            "toString", new PropertyMeta(true))
        {
            _types = types;
        }

        public override IDataObject Call(IDataObject[] args)
        {
            var internals = string.Join(",",
                args[0].Type.Properties
                    .Where(p => p.HasValue(args[0]))
                    .Select(p => p.GetValue(args[0]))
                    .Select(o => o.Type.GetProperty(Name, Meta)));
            return _types[Primitives.String].CreateInstance($"{args[0].Type} {{{internals}}}");
        }
    }
}