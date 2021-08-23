using System.Collections.Generic;
using Cat.data.exceptions;

namespace Cat.data.types
{
    public interface ITypeStorage
    {
        public DataType GetOrCreateType(string name, string fullName, List<IDataMember> members);

        public void ClearTypes();
    }

    public class TypeStorage : ITypeStorage
    {
        public Dictionary<string, DataType> Types = new();

        public DataType GetOrCreateType(string name, string fullName, List<IDataMember> members)
        {
            var createdType = CreateType(name, fullName, members);
            if (Types.ContainsKey(fullName))
            {
                var cachedType = Types[fullName];
                if (!cachedType.IsEquivalentTo(createdType))
                    throw new CatTypeRedeclarationException(cachedType, createdType);
                
                createdType.Dispose();
                return cachedType;
            }

            Types[fullName] = createdType;

            return createdType;
        }

        public void ClearTypes()
        {
            Types.Clear();
        }

        private DataType CreateType(string name, string fullName, List<IDataMember> members)
        {
            return new DataType(name, fullName, members, this);
        }
    }
}