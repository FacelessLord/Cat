using System.Collections.Generic;
using Cat.data.exceptions;
using Cat.data.properties;
using Cat.data.properties.api;
using Cat.data.types.api;
using Cat.data.types.primitives.@object;

namespace Cat.data.types.primitives
{
    public class AnyType : ObjectType
    {
        private const string TypeName = "Any";
        private const string TypeFullName = TypesPaths.System + TypeName;

        public AnyType(TypeStorage types) : base(types)
        {
            Name = TypeName;
            FullName = TypeFullName;
            Properties = new HashSet<IDataProperty>();
        }


        public HashSet<IDataProperty> Properties { get; }
        public string Name { get; }
        public string FullName { get; }

        public bool IsAssignableFrom(IDataType type)
        {
            return true;
        }

        public bool IsAssignableTo(IDataType type)
        {
            return false;
        }

        public bool IsEquivalentTo(IDataType type)
        {
            return type.Properties.Count == 0;
        }

        public IEnumerable<IDataProperty> GetProperty(string name, PropertyMeta meta)
        {
            return System.Array.Empty<IDataProperty>();
        }

        public bool HasProperty(string name, PropertyMeta meta)
        {
            return false;
        }
    }
}