using CatApi.interpreting;

namespace CatImplementations.ast.nodes.modules
{
    public class ModuleImportNode : INode
    {
        public IdNode Target { get; }

        public ModuleImportNode(IdNode target)
        {
            Target = target;
        }
    }
}