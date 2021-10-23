using System.Collections.Generic;
using System.Linq;
using CatApi.objects.primitives;
using CatApi.structures;
using CatApi.structures.properties;
using CatApi.types;
using CatApi.types.containers;
using CatApi.types.dataTypes;
using CatApi.types.primitives;
using CatApi.types.primitives.@object;
using CatApi.utils;

namespace CatApi.bindings
{
    public class PrimitivesMethodsBindings
    {
        public PrimitivesMethodsBindings(TypeStorage typeStorage, IEnumerable<AbstractMethodBinding> bindings)
        {
            Bindings = bindings.ToHashSet();
            ObjectDataType.BaseMethods = Bindings
                .Where(b => ObjectDataType.BaseMethodNames.Contains(b.Name))
                .Select(b => new BoundMethod(b) as ObjectMethod)
                .ToHashSet();
            ObjectDataType.BaseProperties = Bindings
                .Where(b => ObjectDataType.BaseMethodNames.Contains(b.Name))
                .Select(b => new DataProperty(b.Type, b.Name))
                .Cast<IDataProperty>()
                .ToHashSet();
        }

        public HashSet<AbstractMethodBinding> Bindings;
    }
}