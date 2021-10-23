using System;
using CatApi.exceptions;
using CatApi.objects;
using CatApi.types;

namespace CatApi.structures.properties
{
    public class DataProperty : IDataProperty
    {
        public DataProperty(TypeBox typeBox, string name)
        {
            DataType = typeBox;
            Name = name;
        }

        public TypeBox DataType { get; }
        public string Name { get; }
        public int AccessRights { get; } = (int) (AccessRight.Read | AccessRight.Write | AccessRight.Call);

        public IDataObject GetValue(IDataObject owner)
        {
            if ((AccessRights & (int) AccessRight.Read) != 0)
                return owner.GetProperty(Name);
            throw new CatIllegalPropertyAccessException(AccessRight.Read, Name);
        }

        public bool HasValue(IDataObject owner)
        {
            return owner.HasProperty(Name);
        }

        public void SetValue(IDataObject owner, IDataObject value)
        {
            if ((AccessRights & (int) AccessRight.Write) != 0)
                owner.SetProperty(Name, value);
            throw new CatIllegalPropertyAccessException(AccessRight.Write, Name);
        }

        public void CallValue(IDataObject owner, params IDataObject[] args)
        {
            if ((AccessRights & (int) AccessRight.Call) != 0)
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