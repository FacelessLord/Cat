using Cat.data.objects.api;
using Cat.data.types.api;

namespace CatModules.api
{
    public interface IModuleMember
    {
        public IDataType ObjectType { get; }
        public IDataObject GetValue();
    }
}