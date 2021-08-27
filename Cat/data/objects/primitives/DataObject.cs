using System.Collections.Generic;
using Cat.data.exceptions;
using Cat.data.objects.api;
using Cat.data.properties;
using Cat.data.properties.api;
using Cat.data.types.api;

namespace Cat.data.objects.primitives
{
    public class DataObject : IDataObject
    {
        public IDataType Type { get; }
        public Dictionary<string, IDataObject> PropertyValues { get; private set; }

        public DataObject(IDataType type)
        {
            Type = type;
        }

        public void SetupProperties(Dictionary<string, IDataObject> propertyValues)
        {
            PropertyValues = propertyValues;
        }

        public IDataObject GetProperty(IDataProperty property)
        {
            return PropertyValues[property.Name];
        }

        public bool HasProperty(IDataProperty property)
        {
            return PropertyValues.ContainsKey(property.Name);
        }

        public void SetProperty(IDataProperty property, IDataObject value)
        {
            PropertyValues[property.Name] = value;
        }

        public IDataObject CallProperty(IDataProperty dataProperty, IDataObject[] args)
        {
            var propertyValue = PropertyValues[dataProperty.Name];
            if (propertyValue is ICallableDataObject callable)
                return callable.Call(args);
            throw new CatIllegalPropertyAccessException(AccessRight.Call, Type, dataProperty);
        }
    }
}