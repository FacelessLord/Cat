using System.Collections.Generic;
using Cat.data.properties.api;
using Cat.data.types.api;
using Cat.data.types.primitives.@object;

namespace Cat.data.types.primitives
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