using System;
using Cat.data.properties;
using Cat.data.properties.api;
using Cat.data.types.api;

namespace Cat.data.exceptions
{
    public class CatIllegalPropertyAccessException : Exception
    {
        public CatIllegalPropertyAccessException(AccessRight accessRight, IDataType ownerType, IDataProperty propertyToAccess) :
            base($"Cannot {AccessRightHelper.AccessRightToString(accessRight)} " +
                 $"property \"{propertyToAccess.Name}\" of type {ownerType.FullName}")
        {
        }
    }
}