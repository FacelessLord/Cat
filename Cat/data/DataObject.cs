using System.Collections.Generic;
using System.Linq;
using Cat.data.exceptions;
using Cat.data.types;

namespace Cat.data
{
    public interface IDataObject
    {
        public DataType Type { get; }
        public DataObject GetMember(IDataMember member);
        public void SetMember(IDataMember member, DataObject value);
    }

    public class DataObject : IDataObject
    {
        public DataType Type { get; }
        private Dictionary<string, DataObject> MemberValues { get; }

        public DataObject(DataType type, Dictionary<string, DataObject> memberValues)
        {
            Type = type;
            if (Config.IsStrongTypingEnabled) // todo write test that creates typeException
            {
                if (!memberValues.Keys.All(type.MemberNames.Contains))
                    throw new CatTypeInstantiationException(type, type.MemberNames.Except(memberValues.Keys));
            }

            MemberValues = memberValues;
        }

        public DataObject GetMember(IDataMember member)
        {
            return MemberValues[member.Name];
        }

        public void SetMember(IDataMember member, DataObject value)
        {
            MemberValues[member.Name] = value;
        }
    }
}