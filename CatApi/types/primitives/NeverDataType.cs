using System.Collections.Generic;
using CatApi.structures;
using CatApi.types.containers;
using CatApi.types.primitives.@object;

namespace CatApi.types.primitives
{
    public class NeverDataType : ObjectDataType
    {
        public const string TypeName = "Never";
        public const string TypeFullName = TypesPaths.System + "." + TypeName;

        public NeverDataType()
        {
            Name = TypeName;
            FullName = TypeFullName;
            Properties = new HashSet<IDataProperty>();
        }

        public HashSet<IDataProperty> Properties { get; }
        public string Name { get; }
        public string FullName { get; }
    }
}