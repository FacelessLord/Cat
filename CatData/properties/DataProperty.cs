using System;
using Cat.data.exceptions;
using Cat.data.objects.api;
using Cat.data.properties.api;
using Cat.data.types.api;

namespace Cat.data.properties
{
    public class DataProperty : IDataProperty
    {
        public DataProperty(IDataType type, string name)
        {
            Type = type;
            Name = name;
        }

        public IDataType Type { get; }
        public string Name { get; }
        public AccessRight AccessRights { get; } = AccessRight.Read | AccessRight.Write | AccessRight.Call;

        public IDataObject GetValue(IDataObject owner)
        {
            if ((AccessRights & AccessRight.Read) != 0)
                return owner.GetProperty(Name);
            throw new CatIllegalPropertyAccessException(AccessRight.Read, Name);
        }

        public bool HasValue(IDataObject owner)
        {
            return owner.HasProperty(Name);
        }

        public void SetValue(IDataObject owner, IDataObject value)
        {
            if ((AccessRights & AccessRight.Write) != 0)
                owner.SetProperty(Name, value);
            throw new CatIllegalPropertyAccessException(AccessRight.Write, Name);
        }

        public void CallValue(IDataObject owner, params IDataObject[] args)
        {
            if ((AccessRights & AccessRight.Call) != 0)
                owner.CallProperty(Name, args);
            throw new CatIllegalPropertyAccessException(AccessRight.Call, Name);
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