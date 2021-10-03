using CatApi.interpreting;
using CatImplementations.ast;

namespace CatApi.modules
{
    public interface IModuleObjectNode : INode
    {
        public AccessModifier AccessModifier { get;}
        public string Name { get; }
        public void SetAccessModifier(AccessModifier value);
    }
}