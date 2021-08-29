using System;
using System.Linq;
using Cat.data.objects.api;
using Cat.data.objects.primitives;

namespace Cat.data.types.primitives.@object
{
    public class ToStringMethod : ObjectMethod
    {
        private readonly ITypeStorage _types;

        public ToStringMethod(ITypeStorage types) : base(
            types.Function(types[Primitives.Object], types[Primitives.String]),
            "toString")
        {
            _types = types;
        }

        public override IDataObject Call(IDataObject[] args)
        {
            var internals = string.Join(",",
                args[0].Properties
                    .Select(kvp => $"{kvp.Key} : {Stringify(kvp.Value)}"));
            return _types[Primitives.String].CreateInstance($"{args[0].Type} {{{internals}}}");
        }

        private string Stringify(IDataObject dataObject)
        {
            if (dataObject.HasProperty(Name))
            {
                var toStringResult = dataObject.CallProperty(Name, Array.Empty<IDataObject>());
                if (toStringResult is StringDataObject s)
                {
                    return s.Value;
                }
            }

            return (Call(new[] { dataObject }) as StringDataObject)!.Value;
        }
    }
}