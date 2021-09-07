namespace CatAst.nodes.modules
{
    public class ConstDefinitionNode : IModuleObjectNode
    {
        public string Name { get; }
        public string Type { get; }

        public ConstDefinitionNode(IdNode name, IdNode type)
        {
            Name = name.IdToken;
            Type = type.IdToken;
        }

        public bool Exported { get; set; }

        public void SetExported(bool value)
        {
            Exported = value;
        }
    }
}