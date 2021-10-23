using System.Linq;
using CatApi.objects;
using CatApi.objects.primitives;
using CatApi.types;
using CatApi.types.containers;
using CatApi.types.dataTypes;
using CatApi.types.primitives;

namespace CatApi.bindings.objectBindings
{
    public sealed class ToStringBinding : AbstractMethodBinding
    {
        private readonly TypeStorage _types;

        public ToStringBinding(TypeStorage types) : base("toString",
            types.Function(Primitives.Object, Primitives.String))
        {
            _types = types;
        }

        public override IDataObject Call(IDataObject[] args)
        {
            var obj = args[0];
            var objType = _types[obj.Type];

            var value = "<err>";
            if (PrimitivesNames.All.Contains(obj.Type.Id))
            {
                value = ReadPrimitiveValue(obj);
            }
            else if (objType is CallableDataType cdt)
            {
                value = "(" + string.Join(", ", cdt.SourceTypes.Select(t => t.Id)) + ")"
                        + " -> " + cdt.TargetDataType.Id;
                return new StringDataObject(value);
            }
            else
            {
                var internals = string.Join(", ",
                    args[0].Properties
                        .Select(kvp => $"{kvp.Key} : {Stringify(kvp.Value)}"));
                value = $"{args[0].Type.Id} {{{internals}}}";
            }

            return new StringDataObject(value);
        }

        private string ReadPrimitiveValue(IDataObject dataObject)
        {
            return dataObject switch
            {
                StringDataObject s => "\"" + s.Value + "\"",
                //todo
                _ => Stringify(dataObject)
            };
        }

        private string Stringify(IDataObject dataObject)
        {
            if (dataObject.HasProperty(Name) && dataObject.Properties[Name] is not BoundMethod)
            {
                var toStringResult = dataObject.CallProperty(Name, new[] { dataObject });
                if (toStringResult is StringDataObject s)
                {
                    return s.Value;
                }
            }

            var interns = string.Join(", ",
                dataObject.Properties.Select(m => $"{m.Key}: {((StringDataObject) Call(new[] { m.Value })).Value}"));
            return
                $"{dataObject.Type.Id} {{{interns}}}";
        }
    }
}