using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CatApi.assumptions;
using CatApi.bindings;
using CatApi.exceptions;
using CatApi.objects;
using CatApi.objects.primitives;
using CatApi.types.dataTypes;
using CatApi.types.primitives;
using CatApi.types.primitives.@bool;
using CatApi.types.primitives.number;
using CatApi.types.primitives.@object;
using CatApi.types.primitives.@string;

namespace CatApi.types.containers
{
    public class TypeStorage
    {
        public Dictionary<string, IDataType> Types = new();

        public Dictionary<(string, string), bool> AssignabilityCache = new();


        public TypeStorage()
        {
            SetupPrimitiveTypes();
        }

        private void SetupPrimitiveTypes()
        {
            // Types[Primitives.Any] = new AnyType(this);
            Types[PrimitivesNames.Never] = new NeverDataType();
            Types[PrimitivesNames.Object] = new ObjectDataType();
            Types[PrimitivesNames.String] = new StringDataType();
            Types[PrimitivesNames.Number] = new NumberDataType();
            Types[PrimitivesNames.Bool] = new BoolDataType();
        }

        public IDataType this[string id] => GetType(id);

        public IDataType this[TypeBox box] => GetType(box.Id);

        public IDataType GetType(string id)
        {
            if (Types.ContainsKey(id))
            {
                return Types[id];
            }

            throw new CatTypeNotFoundException(id);
        }

        public bool AreTypesEquivalent(IDataType a, IDataType b)
        {
            return IsTypeAssignableFrom(a, b) && IsTypeAssignableFrom(b, a);
        }

        public void RegisterType(IDataType type)
        {
            if (Types.ContainsKey(type.FullName))
                throw new CatTypeRedeclarationException(Types[type.FullName], type);
            Types[type.FullName] = type;
        }

        public bool IsTypeAssignableFrom(IDataType a, IDataType b)
        {
            if (b.FullName == NeverDataType.TypeFullName || a.FullName == ObjectDataType.TypeFullName)
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
                                      Variancy.Contravariant => IsTypeAssignableFrom(this[b.ConstructorTypes[p.i].type],
                                          this[p.act.type]),
                                      Variancy.Covariant => IsTypeAssignableFrom(this[p.act.type],
                                          this[b.ConstructorTypes[p.i].type]),
                                      _ => AreTypesEquivalent(this[p.act.type], this[b.ConstructorTypes[p.i].type])
                                  })
                          && a.Properties.All(property => b.Properties
                              .Any(m => property.Name == m.Name &&
                                        IsTypeAssignableFrom(this[property.DataType], this[m.DataType])));
            if (theorem)
            {
                assumption.GotToBeTrue();
                return true;
            }

            assumption.GotToBeWrong();
            return false;
        }


        public void ClearTypes()
        {
            Types.Clear();
            SetupPrimitiveTypes();
        }

        public TypeBox Intersection(TypeBox a, TypeBox b)
        {
            if (String.Compare(a.Id, b.Id, StringComparison.Ordinal) > 0)
                (a, b) = (b, a);
            return new TypeBox(IntersectionName(a, b));
        }

        private static string IntersectionName(TypeBox a, TypeBox b)
        {
            return $"{a.Id}&{b.Id}";
        }

        public TypeBox Union(TypeBox a, TypeBox b)
        {
            if (String.Compare(a.Id, b.Id, StringComparison.Ordinal) > 0)
                (a, b) = (b, a);
            return new TypeBox(UnionName(a, b));
        }

        private static string UnionName(TypeBox a, TypeBox b)
        {
            return $"{a.Id}&{b.Id}";
        }

        public TypeBox Function(params TypeBox[] args)
        {
            var typeBox = new TypeBox(FunctionName(args));
            if (!Types.ContainsKey(typeBox.Id))
            {
                Types[typeBox.Id] = new CallableDataType(args);
            }
            return new(FunctionName(args));
        }

        public static string FunctionName(params TypeBox[] args) =>
            $"({string.Join(", ", args.Take(args.Length - 1).Select(t => t.Id))}) -> {args[^1].Id}";

        public IDataObject CreateTypeInstance(IDataType type, params IDataObject[] args)
        {
            var box = TypeBox.From(type);
            var dataObject = new DataObject(box);
            type.PopulateObject(dataObject, args);

            return dataObject;
        }
    }
}