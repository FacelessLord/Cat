using System;
using Cat.data.properties;
using Cat.data.types.api;

namespace Cat.data.exceptions
{
    public class CatIllegalPropertyAccessException : Exception
    {
        public CatIllegalPropertyAccessException(AccessRight accessRight, IDataType ownerType, string name) :
            base($"Cannot {AccessRightHelper.AccessRightToString(accessRight)} " +
                 $"property \"{name}\" of type {ownerType.FullName}")
        {
        }
    }
}