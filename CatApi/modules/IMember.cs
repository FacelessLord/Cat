using CatApi.objects;
using CatApi.types;

namespace CatApi.modules
{
    public interface IMember
    {
        public string Name { get; }
        public AccessModifier AccessModifier { get; }
    }
}