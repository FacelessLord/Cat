using System;
using System.Collections.Generic;
using System.Linq;

namespace Cat.data.types
{
    public class DataType : IDisposable
    {
        private readonly ITypeStorage _typeStorage;
        public HashSet<IDataMember> Members { get; }
        public HashSet<string> MemberNames { get; }
        public string Name { get; }
        public string FullName { get; }

        public DataType(string name, string fullName, List<IDataMember> members, ITypeStorage typeStorage)
        {
            _typeStorage = typeStorage;
            Name = name;
            members.ForEach(m => m.SetDeclaringType(this));
            Members = members.ToHashSet();
            MemberNames = members.Select(m => m.Name).ToHashSet();
            FullName = fullName;
        }


        public bool IsAssignableFrom(DataType type)
        {
            foreach (var member in Members)
            {
                if (!type.Members.Any(m => m.IsReplaceableBy(member)))
                    return false;
            }

            return true;
        }

        public bool IsAssignableTo(DataType type)
        {
            return type.IsAssignableFrom(this);
        }

        public bool IsEquivalentTo(DataType type)
        {
            return IsAssignableTo(type) && IsAssignableFrom(type);
        }

        public DataType Intersection(DataType b)
        {
            return _typeStorage.GetOrCreateType($"{Name}&{b.Name}", $"{FullName}&{b.FullName}",
                Members.Intersect(b.Members).ToList());
        }

        public DataType Union(DataType b)
        {
            return _typeStorage.GetOrCreateType($"{Name}|{b.Name}", $"{FullName}|{b.FullName}",
                Members.Union(b.Members).ToList());
        }

        public override string ToString()
        {
            return $"{Name}{{{string.Join(", ", Members.Select(m => $"{m.Name}:{m.Type.Name}"))}}}";
        }


        public void Dispose()
        {
            Members.Clear();
        }
    }
}