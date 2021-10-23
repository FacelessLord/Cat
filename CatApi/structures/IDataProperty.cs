using CatApi.objects;
using CatApi.types;

namespace CatApi.structures
{
    public interface IDataProperty
    {
        public TypeBox DataType { get; }
        public string Name { get; }
        public int AccessRights { get; }
        public IDataObject GetValue(IDataObject owner);
        public void SetValue(IDataObject owner, IDataObject value);
        public void CallValue(IDataObject owner, params IDataObject[] args);
    }
}