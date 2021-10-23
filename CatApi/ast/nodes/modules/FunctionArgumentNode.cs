using CatApi.interpreting;

namespace CatApi.ast.nodes.modules
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