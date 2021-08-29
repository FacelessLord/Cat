using System.Collections.Generic;
using Cat.data.exceptions;
using Cat.data.objects.api;
using Cat.data.properties;
using Cat.data.types.api;

namespace Cat.data.objects.primitives
{
    public class DataObject : IDataObject
    {
        public Dictionary<string, IDataObject> Properties { get; set; }
        public IDataType Type { get; }

        public DataObject(IDataType type)
        {
            Type = type;
        }

        public void SetupProperties(Dictionary<string, IDataObject> propertyValues)
        {
            Properties = propertyValues;
        }

        public IDataObject GetProperty(string property)
        {
            return Properties[property];
        }

        public bool HasProperty(string property)
        {
            return Properties.ContainsKey(property);
        }

        public void SetProperty(string property, IDataObject value)
        {
            Properties[property] = value;
        }

        public IDataObject CallProperty(string property, IDataObject[] args)
        {
            var propertyValue = Properties[property];
            if (propertyValue is ICallableDataObject callable)
            {
                return callable.Call(args);
            }

            throw new CatIllegalPropertyAccessException(AccessRight.Call, Type, property);
        }
    }
}