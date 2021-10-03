using CatApi.interpreting;

namespace CatImplementations.ast.nodes.modules
{
    public class FunctionArgumentNode : INode
    {
        public string Id { get; }
        public string TypeName { get; }

        public FunctionArgumentNode(IdNode id, IdNode typeName)
        {
            Id = id.IdToken;
            TypeName = typeName.IdToken;
        }
    }
}