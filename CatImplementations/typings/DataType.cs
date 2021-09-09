using System;
using System.Collections.Generic;
using System.Linq;
using CatApi.structures;
using CatApi.types;

namespace CatImplementations.typings
{
    public abstract class DataType : IDisposable, IDataType
    {
        public HashSet<string> PropertyNames { get; set; }
        private HashSet<IDataProperty> properties = new HashSet<IDataProperty>();

        public HashSet<IDataProperty> Properties
        {
            get => properties;
            set
            {
                properties = value.ToHashSet();
                PropertyNames = value.Select(m => m.Name).ToHashSet();
            }
        }

        public List<(IDataType, Variancy)> ConstructorTypes { get; set; } = new();

        public string Name { get; set; }
        public string FullName { get; set; }

        public DataType(string name, string fullName)
        {
            Name = name;
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

        public abstract void PopulateObject(IDataObject dataObject);
        public abstract IDataObject CreateInstance(params object[] args);

        public override string ToString()
        {
            return $"{Name}{{{string.Join(", ", Properties.Select(m => $"{m.Name}:{m.Type.Name}"))}}}";
        }

        public void Dispose()
        {
            Properties.Clear();
        }

        public abstract IDataObject NewInstance(params object[] args);
    }
}