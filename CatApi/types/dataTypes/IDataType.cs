using System.Collections.Generic;
using CatApi.objects;
using CatApi.structures;

namespace CatApi.types.dataTypes
{
    public interface IDataType
    {
        HashSet<IDataProperty> Properties
        {
            get;
        }

        public List<(TypeBox type, Variancy variancy)> ConstructorTypes { get; set; }

        string FullName { get; }

        public IDataProperty GetProperty(string name);
        public bool HasProperty(string name);

        public void PopulateObject(IDataObject dataObject, params IDataObject[] args);
    }

    public enum Variancy
    {
        Covariant,
        Contravariant,
        Invariant
    }
}