namespace CatAst.nodes.modules
{
    public class ModuleDeclarationNode : INode
    {
        public string ModuleName { get; }

        public ModuleDeclarationNode(IdNode moduleName)
        {
            ModuleName = moduleName.IdToken;
        }
    }
}