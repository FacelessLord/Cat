using System;
using System.Collections.Generic;
using Cat.data.types;

namespace Cat.data.exceptions
{
    public class CatTypeInstantiationException : Exception
    {
        public CatTypeInstantiationException(DataType type, IEnumerable<string> missedMembers) :
            base($"Type {type.FullName} requires {string.Join(", ", missedMembers)} members to be implemented")
        {
        }
    }
}