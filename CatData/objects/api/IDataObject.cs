using System.Collections.Generic;
using Cat.data.types.api;

namespace Cat.data.objects.api
{
    public interface IDataObject
    {
        public Dictionary<string, IDataObject> Properties { get; }
        public IDataType Type { get; }
        public IDataObject GetProperty(string name);
        public bool HasProperty(string name);
        public void SetProperty(string name, IDataObject value);
        public IDataObject CallProperty(string name, IDataObject[] args);
        public void SetupProperties(Dictionary<string, IDataObject> propertyValues);
    }
}