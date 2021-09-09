using CatApi.objects;
using CatApi.types;

namespace CatApi.modules
{
    public interface IModuleMember
    {
        public string Name { get; }
        public IDataType ObjectType { get; }
        public IDataObject GetValue();
    }
}