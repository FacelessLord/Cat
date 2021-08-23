using System;
using System.Security;
using Cat.data.exceptions;
using Cat.data.types;

namespace Cat.data
{
    public interface IDataMember
    {
        public DataType Type { get; }
        public string Name { get; }

        public AccessRight AccessRights { get; }

        public DataObject Get(DataObject owner);
        public void Set(DataObject owner, DataObject value);
        public void SetDeclaringType(DataType type);
        public bool IsReplaceableBy(IDataMember replacement);
    }

    public class DataMember : IDataMember
    {
        public DataMember(DataType type, string name)
        {
            Type = type;
            Name = name;
        }

        /**
         * Type where this member declared
         */
        public DataType DeclaringType { get; private set; }

        public DataType Type { get; }
        public string Name { get; }
        public AccessRight AccessRights { get; } = AccessRight.Read | AccessRight.Write | AccessRight.Call;

        public DataObject Get(DataObject owner)
        {
            if ((AccessRights & AccessRight.Read) != 0)
                return owner.GetMember(this);
            throw new CatIllegalAccessException(AccessRight.Read, DeclaringType, this);
        }

        public void Set(DataObject owner, DataObject value)
        {
            if ((AccessRights & AccessRight.Write) != 0)
                owner.SetMember(this, value);
            throw new CatIllegalAccessException(AccessRight.Write, DeclaringType, this);
        }

        public void SetDeclaringType(DataType type)
        {
            if (DeclaringType != null)
                throw new SecurityException("Declaring type can be set only once");
            DeclaringType = type;
        }

        public bool IsReplaceableBy(IDataMember replacement)
        {
            return Name == replacement.Name && replacement.Type.IsAssignableFrom(Type) && (AccessRights & replacement.AccessRights) == AccessRights;
        }
    }

    [Flags]
    public enum AccessRight
    {
        Read = 1,
        Write = 2,
        Call = 4
    }

    public static class AccessRightHelper
    {
        public static string AccessRightToString(AccessRight right)
        {
            return right switch
            {
                AccessRight.Read => nameof(AccessRight.Read),
                AccessRight.Write => nameof(AccessRight.Write),
                AccessRight.Call => nameof(AccessRight.Call),
                _ => throw new ArgumentOutOfRangeException(nameof(right), right, null)
            };
        }
    }
}