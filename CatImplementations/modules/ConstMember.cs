using CatApi.modules;
using CatImplementations.ast;

namespace CatImplementations.modules
{
    public class ConstMember : IMember
    {
        public ConstMember(string name, AccessModifier accessModifier)
        {
            Name = name;
            AccessModifier = accessModifier;
        }

        public string Name { get; }
        public AccessModifier AccessModifier { get; }
    }
}