using System;
using System.Collections.Generic;
using System.Linq;
using Cat.data.objects;
using Cat.data.objects.api;
using Cat.data.properties;
using Cat.data.properties.api;
using Cat.data.types.api;

namespace Cat.data.types
{
    public abstract class DataType : IDisposable, IDataType
    {
        public HashSet<string> PropertyNames { get; set; }
        private HashSet<IDataProperty> properties;
        public HashSet<IDataProperty> Properties
        {
            get => properties ;
            set
            {
                value.ToList().ForEach(m => m.SetDeclaringType(this));
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
            return from.Properties.All(property => to.Properties.Any(m => m.IsReplaceableBy(property)));
        }

        public virtual bool IsAssignableTo(IDataType type)
        {
            return type.IsAssignableFrom(this);
        }

        public virtual bool IsEquivalentTo(IDataType type)
        {
            return IsAssignableTo(type) && IsAssignableFrom(type);
        }

        public virtual IEnumerable<IDataProperty> GetProperty(string name, PropertyMeta meta)
        {
            return FindPropertiesInSet(Properties, name, meta); // todo add proper ordering from more to less specific
        }

        public static IEnumerable<IDataProperty> FindPropertiesInSet(HashSet<IDataProperty> properties, string name,
            PropertyMeta meta)
        {
            return properties.Where(m => m.Name == name).Where(m => m.Meta.Callable || !meta.Callable)
                .Where(m => m.Meta.ArgTypes.Count == meta.ArgTypes.Count)
                .Where(m => m.Meta.ArgTypes.Select((t, i) => (t, i))
                    .All(p => p.t.IsAssignableFrom(meta.ArgTypes[p.i])));
        }

        public virtual bool HasProperty(string name, PropertyMeta meta)
        {
            return GetProperty(name, meta).Any();
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
    }
}