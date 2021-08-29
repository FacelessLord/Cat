﻿using System;
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
            var obj = args[0];

            var value = "<err>";
            if (Primitives.All.Contains(obj.Type.Name))
            {
                value = ReadPrimitiveValue(obj);
            }
            else if (obj.Type is CallableDataType cdt)
            {
                value = "(" + string.Join(", ", cdt.SourceTypes.Select(t => t.Name)) + ")"
                        + " -> " + cdt.TargetType.Name;
                return _types[Primitives.String].CreateInstance(value);
            }
            else
            {
                var internals = string.Join(", ",
                    args[0].Properties
                        .Select(kvp => $"{kvp.Key} : {Stringify(kvp.Value)}"));
                value = $"{args[0].Type.Name} {{{internals}}}";
            }

            return _types[Primitives.String].CreateInstance(value);
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
            if (dataObject.HasProperty(Name))
            {
                var toStringResult = dataObject.CallProperty(Name, new[] { dataObject });
                if (toStringResult is StringDataObject s)
                {
                    return s.Value;
                }
            }

            return (Call(new[] { dataObject }) as StringDataObject)!.Value;
        }
    }
}