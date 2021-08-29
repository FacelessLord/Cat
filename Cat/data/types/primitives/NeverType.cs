using System.Collections.Generic;
using Cat.data.properties.api;
using Cat.data.types.api;
using Cat.data.types.primitives.@object;

namespace Cat.data.types.primitives
{
    public class NeverType : ObjectType
    {
        private const string TypeName = "Never";
        private const string TypeFullName = TypesPaths.System + "." + TypeName;

        public NeverType(TypeStorage types) : base(types)
        {
            Name = TypeName;
            FullName = TypeFullName;
            Properties = new HashSet<IDataProperty>();
        }

        public HashSet<IDataProperty> Properties { get; }
        public string Name { get; }
        public string FullName { get; }

        public override bool IsAssignableFrom(IDataType type)
        {
            return false;
        }

        public override bool IsAssignableTo(IDataType type)
        {
            return true;
        }

        public override bool IsEquivalentTo(IDataType type)
        {
            return type is NeverType;
        }
    }
}