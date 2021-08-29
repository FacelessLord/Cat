using System;
using System.Collections.Generic;
using System.Linq;
using Cat.data.exceptions;
using Cat.data.objects.api;
using Cat.data.properties;
using Cat.data.properties.api;
using Cat.data.types.api;
using Cat.data.types.primitives;
using Cat.data.types.primitives.@bool;
using Cat.data.types.primitives.number;
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


        public IDataType this[string fullName] { get; }
    }

    public class TypeStorage : ITypeStorage
    {
        public Dictionary<string, IDataType> Types = new();

        public IDataType this[string fullName] =>
            GetOrCreateType(fullName, () => throw new CatTypeNotFoundException(fullName));

        public TypeStorage()
        {
            // Types[Primitives.Any] = new AnyType(this);
            Types[Primitives.Never] = new NeverType();
            Types[Primitives.Object] = new ObjectType();
            Types[Primitives.String] = new StringType();
            Types[Primitives.Number] = new NumberType();
            Types[Primitives.Bool] = new BoolType();

            SetupBaseMethods();
        }

        private void SetupBaseMethods()
        {
            SetupObjectBaseMethods();
        }

        private void SetupObjectBaseMethods()
        {
            var objectType = ((ObjectType) Types[Primitives.Object]);
            objectType.BaseMethods = new HashSet<ObjectMethod>()
            {
                new ToStringMethod(this)
            };
            objectType.Properties = objectType.BaseMethods
                .Select(m => new DataProperty(m.Type, m.Name) as IDataProperty)
                .ToHashSet();
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
                () => new ObjectType()
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
                () => new ObjectType()
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
                () => new CallableDataType(args));
        }
    }
}