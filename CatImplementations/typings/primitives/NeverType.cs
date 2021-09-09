using System.Collections.Generic;
using CatApi.structures;
using CatImplementations.typings.primitives.@object;

namespace CatImplementations.typings.primitives
{
    public class NeverType : ObjectType
    {
        public const string TypeName = "Never";
        public const string TypeFullName = TypesPaths.System + "." + TypeName;

        public NeverType()
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