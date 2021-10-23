using System;
using CatApi.structures.properties;

namespace CatApi.exceptions
{
    public class CatIllegalPropertyAccessException : Exception
    {
        public CatIllegalPropertyAccessException(AccessRight right, string name) :
            base($"Cannot {AccessRightHelper.AccessRightToString(right)} " +
                 $"property \"{name}\" of unknown type")
        {
        }
    }
}