using System;
using Cat.data.exceptions;
using Cat.data.objects.api;
using Cat.data.properties.api;
using Cat.data.types.api;

namespace Cat.data.properties
{
    public class DataProperty : IDataProperty
    {
        public DataProperty(IDataType type, PropertyMeta meta, string name)
        {
            Type = type;
            Meta = meta;
            Name = name;
        }

        /**
         * Type where this property declared
         */
        public IDataType DeclaringType { get; private set; }

        public IDataType Type { get; }
        public PropertyMeta Meta { get; }
        public string Name { get; }
        public AccessRight AccessRights { get; } = AccessRight.Read | AccessRight.Write | AccessRight.Call;

        public IDataObject GetValue(IDataObject owner)
        {
            if ((AccessRights & AccessRight.Read) != 0)
                return owner.GetProperty(this);
            throw new CatIllegalPropertyAccessException(AccessRight.Read, DeclaringType, this);
        }

        public bool HasValue(IDataObject owner)
        {
            return owner.HasProperty(this);
        }

        public void SetValue(IDataObject owner, IDataObject value)
        {
            if ((AccessRights & AccessRight.Write) != 0)
                owner.SetProperty(this, value);
            throw new CatIllegalPropertyAccessException(AccessRight.Write, DeclaringType, this);
        }

        public void CallValue(IDataObject owner, params IDataObject[] args)
        {
            if ((AccessRights & AccessRight.Call) != 0)
                owner.CallProperty(this, args);
            throw new CatIllegalPropertyAccessException(AccessRight.Call, DeclaringType, this);
        }

        public void SetDeclaringType(IDataType type)
        {
            if (DeclaringType != null)
                throw new CatSecurityException("Declaring type can be set only once");
            DeclaringType = type;
        }

        public bool IsReplaceableBy(IDataProperty replacement)
        {
            return Name == replacement.Name && replacement.Type.IsAssignableFrom(Type) &&
                   (AccessRights & replacement.AccessRights) == AccessRights;
        }
    }

    [Flags]
    public enum AccessRight
    {
        Read = 1,
        Write = 2,
        Call = 4,
        Rwc = Read | Write | Call
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