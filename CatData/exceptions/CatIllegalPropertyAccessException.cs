using System;
using Cat.data.properties;
using Cat.data.types.api;

namespace Cat.data.exceptions
{
    public class CatIllegalPropertyAccessException : Exception
    {
        public CatIllegalPropertyAccessException(AccessRight accessRight, string name) :
            base($"Cannot {AccessRightHelper.AccessRightToString(accessRight)} " +
                 $"property \"{name}\" of unknown type")
        {
        }
    }
}