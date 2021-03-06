using CatApi.interpreting;

namespace CatApi.ast.nodes
{
    public class PropertyAccessNode : INode
    {
        public INode Source { get; }
        public string PropertyName { get; }

        public PropertyAccessNode(INode source, IdNode propertyName)
        {
            Source = source;
            PropertyName = propertyName.IdToken;
        }
    }
}