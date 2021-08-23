using System;
using Cat.data.types;

namespace Cat.data.exceptions
{
    public class CatIllegalAccessException : Exception
    {
        public CatIllegalAccessException(AccessRight accessRight, DataType ownerType, DataMember memberToAccess) :
            base($"Cannot {AccessRightHelper.AccessRightToString(accessRight)} " +
                 $"member \"{memberToAccess.Name}\" of type {ownerType.FullName}")
        {
        }
    }
}