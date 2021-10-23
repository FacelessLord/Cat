using System;
using System.Collections.Generic;
using System.Linq;
using CatApi.objects;
using CatApi.objects.primitives;
using CatApi.structures;
using CatApi.types.containers;
using CatApi.types.primitives.@object;

namespace CatApi.types.dataTypes
{
    public class DefinedType : ObjectDataType
    {
        public HashSet<ObjectMethod> BaseMethods = new();

        public DefinedType(string typeName, HashSet<IDataProperty> properties)
        {
            LazyProperties = new Lazy<HashSet<IDataProperty>>(() => BaseProperties.Union(properties).ToHashSet());
            FullName = typeName;
        }

        public Lazy<HashSet<IDataProperty>> LazyProperties;
        public override HashSet<IDataProperty> Properties => LazyProperties.Value;

        public override void PopulateObject(IDataObject dataObject, params IDataObject[] args)
        {
            dataObject.SetupProperties(BaseMethods.ToDictionary(p => p.Name, p => p as IDataObject));
        }
    }
}