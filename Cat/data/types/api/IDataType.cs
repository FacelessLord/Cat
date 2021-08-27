using System.Collections.Generic;
using Cat.data.objects.api;
using Cat.data.properties;
using Cat.data.properties.api;

namespace Cat.data.types.api
{
    public interface IDataType
    {
        HashSet<IDataProperty> Properties { get; }
        string Name { get; }
        string FullName { get; }

        public bool IsAssignableFrom(IDataType type);
        public bool IsAssignableTo(IDataType type);

        public bool IsEquivalentTo(IDataType type);

        public IEnumerable<IDataProperty> GetProperty(string name, PropertyMeta meta);
        public bool HasProperty(string name, PropertyMeta meta);

        public void PopulateObject(IDataObject dataObject);
        public IDataObject CreateInstance(params object[] args);
    }
}