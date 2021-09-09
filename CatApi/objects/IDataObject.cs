using System.Collections.Generic;
using CatApi.types;

namespace CatApi.objects
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