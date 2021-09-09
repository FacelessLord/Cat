using CatApi.interpreting;

namespace CatImplementations.ast.nodes.modules
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