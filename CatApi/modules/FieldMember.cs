using CatApi.interpreting;
using CatApi.types;

namespace CatApi.modules
{
    public class FieldMember : IMember
    {
        public FieldMember(string name, AccessModifier accessModifier, TypeBox type, INode value)
        {
            Name = name;
            AccessModifier = accessModifier;
            Type = type;
            Value = value;
        }

        public string Name { get; }
        public AccessModifier AccessModifier { get; }
        public TypeBox Type { get; }
        public INode Value { get; }
    }
}