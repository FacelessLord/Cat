﻿using System;
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

        public bool IsTypeAssignableFrom(IDataType a, IDataType b);
    }


    public class TypeStorage : ITypeStorage
    {
        public Dictionary<string, IDataType> Types = new();

        public Dictionary<(string, string), bool> AssignabilityCache = new();

        public IDataType this[string fullName] =>
            GetOrCreateType(fullName, () => throw new CatTypeNotFoundException(fullName));

        public bool AreTypesEquivalent(IDataType a, IDataType b)
        {
            return IsTypeAssignableFrom(a, b) && IsTypeAssignableFrom(b, a);
        }

        public bool IsTypeAssignableFrom(IDataType a, IDataType b)
        {
            if (b.FullName == NeverType.TypeFullName || a.FullName == ObjectType.TypeFullName)
            {
                return true;
            }

            var cacheKey = (a.FullName, b.FullName);
            if (AssignabilityCache.ContainsKey(cacheKey))
            {
                return AssignabilityCache[cacheKey];
            }

            using var assumption = Assumption.That(a).IsAssignableFrom(b).On(this);

            var theorem = a.ConstructorTypes.Count <= b.ConstructorTypes.Count
                          && a.ConstructorTypes
                              .Select((act, i) => (act, i))
                              .All(p =>
                                  p.act.variancy switch
                                  {
                                      Variancy.Contravariant => IsTypeAssignableFrom(b.ConstructorTypes[p.i].type,
                                          p.act.type),
                                      Variancy.Covariant => IsTypeAssignableFrom(p.act.type,
                                          b.ConstructorTypes[p.i].type),
                                      _ => AreTypesEquivalent(p.act.type, b.ConstructorTypes[p.i].type)
                                  })
                          && a.Properties.All(property => b.Properties
                              .Any(m => property.Name == m.Name && IsTypeAssignableFrom(m.Type, property.Type)));
            if (theorem)
            {
                assumption.GotToBeTrue();
                return true;
            }

            assumption.GotToBeWrong();
            return false;
        }

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
            var objectType = ((ObjectType) Types[Primitives.Object]);
            objectType.BaseMethods = new HashSet<ObjectMethod>()
            {
                new ToStringMethod(this)
            };

            foreach (var type in Types)
                SetupObjectBaseMethods(type.Value);
        }

        private void SetupObjectBaseMethods(IDataType type)
        {
            type.Properties = ((ObjectType) Types[Primitives.Object]).BaseMethods
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