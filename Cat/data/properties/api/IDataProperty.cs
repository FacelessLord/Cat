using Cat.data.objects.api;
using Cat.data.types.api;

namespace Cat.data.properties.api
{
    public interface IDataProperty
    {
        public IDataType Type { get; }
        public string Name { get; }
        public AccessRight AccessRights { get; }
        public IDataObject GetValue(IDataObject owner);
        public void SetValue(IDataObject owner, IDataObject value);
        public void CallValue(IDataObject owner, params IDataObject[] args);
    }
}