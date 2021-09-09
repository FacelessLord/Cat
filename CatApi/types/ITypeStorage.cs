using System;

namespace CatApi.types
{
    public interface ITypeStorage
    {
        public IDataType GetOrCreateType(string fullName, Func<IDataType> generator);

        public void ClearTypes();

        public IDataType Intersection(IDataType a, IDataType b);

        public IDataType Union(IDataType a, IDataType b);
        public IDataType Function(params IDataType[] args);
        public bool HasType(string typeName);

        public IDataType this[string fullName] { get; }

        public bool IsTypeAssignableFrom(IDataType a, IDataType b);
    }
}