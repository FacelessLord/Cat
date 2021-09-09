using System.Collections.Generic;
using CatApi.objects;
using CatApi.structures;

namespace CatApi.types
{
    public interface IDataType
    {
        HashSet<IDataProperty> Properties { get; set; }

        public List<(IDataType type, Variancy variancy)> ConstructorTypes { get; set; }

        string Name { get; }
        string FullName { get; }

        public IDataProperty GetProperty(string name);
        public bool HasProperty(string name);

        public void PopulateObject(IDataObject dataObject);
        public IDataObject CreateInstance(params object[] args);
    }

    public enum Variancy
    {
        Covariant,
        Contravariant,
        Invariant
    }
}