using System.Collections.Generic;
using Cat.data.types.api;

namespace Cat.data.objects.api
{
    public abstract class ObjectMethod : CallableDataObject
    {
        protected ObjectMethod(IDataType function, string methodName) : base(function)
        {
            Name = methodName;
            Properties = new Dictionary<string, IDataObject>();
        }

        public string Name { get; }
    }
}