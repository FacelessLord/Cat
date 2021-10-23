using CatApi.interpreting;
using CatApi.modules;

namespace CatApi.ast.nodes.modules
{
    public class FieldDefinitionNode : IModuleObjectNode
    {
        public INode Value { get; }
        public AccessModifier AccessModifier { get; private set; } = AccessModifier.Private;
        public string Name { get; }
        public string Type { get; }

        public FieldDefinitionNode(IdNode name, IdNode type, INode value)
        {
            Value = value;
            Name = name.IdToken;
            Type = type.IdToken;
        }
        public void SetAccessModifier(AccessModifier value)
        {
            AccessModifier = value;
        }
    }
}