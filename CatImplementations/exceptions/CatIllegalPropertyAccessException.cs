using System;
using CatImplementations.structures.properties;

namespace CatImplementations.exceptions
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