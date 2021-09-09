using CatApi.interpreting;

namespace CatApi.modules
{
    public interface IModuleObjectNode : INode
    {
        public bool Exported { get; set; }

        public void SetExported(bool value);

        public string Name { get; }
    }
}