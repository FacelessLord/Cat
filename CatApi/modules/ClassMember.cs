using System.Collections.Generic;
using System.Linq;

namespace CatApi.modules
{
    public class ClassMember : IMember
    {
        public ClassMember(string name, AccessModifier accessModifier, IEnumerable<IMember> members)
        {
            Name = name;
            AccessModifier = accessModifier;
            Members = members.ToList();
        }

        public string Name { get; }
        public AccessModifier AccessModifier { get; }
        public List<IMember> Members { get; }
    }
}