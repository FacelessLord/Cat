using System.Collections.Generic;
using Cat.data.properties.api;
using Cat.data.types.api;

namespace Cat.data.objects.api
{
    public interface IDataObject
    {
        public IDataType Type { get; }
        public IDataObject GetProperty(IDataProperty property);
        public bool HasProperty(IDataProperty property);
        public void SetProperty(IDataProperty property, IDataObject value);
        public IDataObject CallProperty(IDataProperty dataProperty, IDataObject[] args);
        public void SetupProperties(Dictionary<string, IDataObject> propertyValues);
    }
}