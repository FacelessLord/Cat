using System;
using CatApi.modules;
using CatApi.objects;
using CatApi.types;
using CatImplementations.objects.primitives;
using CatImplementations.typings.primitives;

namespace CatImplementations.modules
{
    public class FunctionModuleMember : IModuleMember
    {
        private readonly ITypeStorage _typeStorage;

        public FunctionModuleMember(string name, ITypeStorage typeStorage, IDataType constructedType)
        {
            _typeStorage = typeStorage;
            Name = name;
            ConstructedType = constructedType;
            Value = new Lazy<IDataObject>(() => new ClassDataObject(ObjectType, constructedType));
        }

        public string Name { get; }
        public IDataType ConstructedType { get; }

        public IDataType ObjectType
        {
            get => _typeStorage[Primitives.Class];
        }

        private readonly Lazy<IDataObject> Value;

        public IDataObject GetValue()
        {
            return Value.Value;
        }
    }
}