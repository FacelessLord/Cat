using Cat.data.objects.api;
using Cat.data.types.api;

namespace Cat.data.properties.api
{
    public interface IDataProperty
    {
        public IDataType Type { get; }
        public string Name { get; }
        public PropertyMeta Meta { get; }
        public AccessRight AccessRights { get; }
        /**
         * Type where this property declared
         */
        public IDataType DeclaringType { get; }

        public IDataObject GetValue(IDataObject owner);
        public bool HasValue(IDataObject owner);
        public void SetValue(IDataObject owner, IDataObject value);
        public void CallValue(IDataObject owner, params IDataObject[] args);
        public void SetDeclaringType(IDataType type);
        public bool IsReplaceableBy(IDataProperty replacement);
    }
}