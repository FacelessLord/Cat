using System.Collections.Generic;
using System.Linq;
using CatApi.types.containers;
using CatApi.types.primitives.@object;

namespace CatApi.types.dataTypes
{
    public class CallableDataType : ObjectDataType
    {
        public CallableDataType(params TypeBox[] args)
        {
            SourceTypes = args.Take(args.Length - 1).ToArray();
            TargetDataType = args[^1];

            ConstructorTypes = new List<(TypeBox, Variancy)>(SourceTypes.Select(t => (t, Variancy.Contravariant)))
                { (TargetDataType, Variancy.Covariant) };

            FullName = TypeStorage.FunctionName(args);
        }

        public TypeBox[] SourceTypes { get; }
        public TypeBox TargetDataType { get; }
    }
}