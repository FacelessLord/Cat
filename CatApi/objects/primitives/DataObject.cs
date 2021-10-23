using System.Collections.Generic;
using CatApi.exceptions;
using CatApi.structures.properties;
using CatApi.types;

namespace CatApi.objects.primitives
{
    public class DataObject : IDataObject
    {
        public Dictionary<string, IDataObject> Properties { get; set; }
        public TypeBox Type { get; }

        public DataObject(TypeBox typeBox)
        {
            Type = typeBox;
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

            throw new CatIllegalPropertyAccessException(AccessRight.Call, property);
        }
    }
}