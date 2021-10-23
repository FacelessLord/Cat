using System;
using System.Collections.Generic;
using System.Linq;
using CatApi.objects;
using CatApi.structures;

namespace CatApi.types.dataTypes
{
    public abstract class DataType : IDisposable, IDataType
    {
        public HashSet<string> PropertyNames { get; set; }
        
        public abstract HashSet<IDataProperty> Properties { get; }
        public List<(TypeBox, Variancy)> ConstructorTypes { get; set; } = new();

        public string FullName { get; set; }

        public DataType(string fullName)
        {
            FullName = fullName;
        }

        public virtual IDataProperty GetProperty(string name)
        {
            return Properties.Single(p => p.Name == name);
        }

        public virtual bool HasProperty(string name)
        {
            return Properties.Any(p => p.Name == name);
        }

        public abstract void PopulateObject(IDataObject dataObject, params IDataObject[] args);

        public override string ToString()
        {
            return $"{FullName}{{{string.Join(", ", Properties.Select(m => $"{m.Name}:{m.DataType.Id}"))}}}";
        }

        public void Dispose()
        {
            Properties.Clear();
        }
    }
}