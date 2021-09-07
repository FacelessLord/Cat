namespace CatAst.nodes.modules
{
    public class FunctionDefinitionNode : IModuleObjectNode
    {
        public string Name { get; }
        public string ReturnType { get; }

        public FunctionDefinitionNode(IdNode name, IdNode returnType)
        {
            Name = name.IdToken;
            ReturnType = returnType.IdToken;
        }

        public bool Exported { get; set; }

        public void SetExported(bool value)
        {
            Exported = value;
        }
    }
}