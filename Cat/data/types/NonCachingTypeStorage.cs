using System.Collections.Generic;

namespace Cat.data.types
{
    public class NonCachingTypeStorage : ITypeStorage
    {
        public DataType GetOrCreateType(string name, string fullName, List<IDataMember> members)
        {
            return  CreateType(name, fullName, members);
        }

        public void ClearTypes()
        {
        }

        private DataType CreateType(string name, string fullName, List<IDataMember> members)
        {
            return new DataType(name, fullName, members, this);
        }
    }
}