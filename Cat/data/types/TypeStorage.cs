using System;
using System.Collections.Generic;
using System.Linq;
using Cat.data.exceptions;
using Cat.data.types.api;
using Cat.data.types.primitives;
using Cat.data.types.primitives.@object;
using Cat.data.types.primitives.@string;

namespace Cat.data.types
{
    public interface ITypeStorage
    {
        public IDataType GetOrCreateType(string fullName, Func<IDataType> generator);

        public void ClearTypes();

        public IDataType Intersection(IDataType a, IDataType b);

        public IDataType Union(IDataType a, IDataType b);
        public IDataType Function(params IDataType[] args);
    }

    public class TypeStorage : ITypeStorage
    {
        public static TypeStorage Instance = new TypeStorage();
        public Dictionary<string, IDataType> Types;

        public IDataType this[string fullName] =>
            GetOrCreateType(fullName, () => throw new CatTypeNotFoundException(fullName));

        public TypeStorage()
        {
            Types[Primitives.Any] = new AnyType(this);
            Types[Primitives.Never] = new NeverType(this);
            Types[Primitives.Object] = new ObjectType(this);
            Types[Primitives.String] = new StringType(this);
            // Types[Primitives.Number] = new ObjectType(this);
        }

        public IDataType GetOrCreateType(string fullName, Func<IDataType> generator)
        {
            if (Types.ContainsKey(fullName))
            {
                var cachedType = Types[fullName];
                // if (!cachedType.IsEquivalentTo(createdType))
                //     throw new CatTypeRedeclarationException(cachedType, createdType);

                return cachedType;
            }

            Types[fullName] = generator();

            return Types[fullName];
        }

        public void ClearTypes()
        {
            Types.Clear();
        }

        public IDataType Intersection(IDataType a, IDataType b)
        {
            if (a.FullName.CompareTo(b) > 0)
                (a, b) = (b, a);
            return GetOrCreateType(IntersectionFullName(a, b),
                () => new ObjectType(this)
                {
                    Name = IntersectionName(a, b),
                    FullName = IntersectionFullName(a, b),
                    Properties = a.Properties.Intersect(b.Properties).ToHashSet()
                });
        }

        private static string IntersectionFullName(IDataType a, IDataType b)
        {
            return $"{a.FullName}&{b.FullName}";
        }

        private static string IntersectionName(IDataType a, IDataType b)
        {
            return $"{a.Name}&{b.Name}";
        }

        public IDataType Union(IDataType a, IDataType b)
        {
            if (a.FullName.CompareTo(b) > 0)
                (a, b) = (b, a);
            return GetOrCreateType(UnionFullName(a, b),
                () => new ObjectType(this)
                {
                    Name = UnionName(a, b),
                    FullName = UnionFullName(a, b),
                    Properties = a.Properties.Union(b.Properties).ToHashSet()
                });
        }

        private static string UnionFullName(IDataType a, IDataType b)
        {
            return $"{a.FullName}&{b.FullName}";
        }

        private static string UnionName(IDataType a, IDataType b)
        {
            return $"{a.Name}&{b.Name}";
        }

        public IDataType Function(params IDataType[] args)
        {
            return GetOrCreateType(CallableDataType.CreateFunctionFullName(args),
                () => new CallableDataType(this, args));
        }
    }
}