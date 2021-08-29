using System;
using System.Collections.Generic;
using System.Linq;
using Cat.data.objects.api;
using Cat.data.properties.api;
using Cat.data.types.api;

namespace Cat.data.types
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

        public string Name { get; set; }
        public string FullName { get; set; }

        public DataType(string name, string fullName)
        {
            Name = name;
            FullName = fullName;
        }

        public virtual bool IsAssignableFrom(IDataType type)
        {
            return IsAssignableFromTo(this, type);
        }

        public static bool IsAssignableFromTo(IDataType from, IDataType to)
        {
            return from.Properties.All(property => to.Properties
                .Any(m => property.Name == m.Name && m.Type.IsAssignableFrom(property.Type)));
        }

        public virtual bool IsAssignableTo(IDataType type)
        {
            return type.IsAssignableFrom(this);
        }

        public virtual bool IsEquivalentTo(IDataType type)
        {
            return IsAssignableTo(type) && IsAssignableFrom(type);
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