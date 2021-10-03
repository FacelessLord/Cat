using CatApi.modules;

namespace CatImplementations.ast.nodes.modules
{
    public class ClassDefinitionNode : IModuleObjectNode
    {
        public ClassMemberListNode Members { get; }
        public AccessModifier AccessModifier { get; set; } = AccessModifier.Private;
        public string Name { get; }

        public ClassDefinitionNode(IdNode name, ClassMemberListNode members)
        {
            Members = members;
            Name = name.IdToken;
        }

        public void SetAccessModifier(AccessModifier value)
        {
            AccessModifier = value;
        }
    }
}