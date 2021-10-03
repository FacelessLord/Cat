using CatApi.objects;
using CatApi.types;
using CatImplementations.ast;

namespace CatApi.modules
{
    public interface IMember
    {
        public string Name { get; }
        public AccessModifier AccessModifier { get; }
    }
}