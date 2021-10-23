using CatApi.modules;

namespace CatApi.ast.nodes.modules
{
    public class ClassDefinitionNode : IModuleObjectNode
    {
        public IModuleObjectNode[] Members { get; }
        public AccessModifier AccessModifier { get; set; } = AccessModifier.Private;
        public string Name { get; }

        public ClassDefinitionNode(IdNode name, ClassMemberListNode members)
        {
            Name = name.IdToken;
            Members = members.Members;
        }

        public void SetAccessModifier(AccessModifier value)
        {
            AccessModifier = value;
        }
    }
}