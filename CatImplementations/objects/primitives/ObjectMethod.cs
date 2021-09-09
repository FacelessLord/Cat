using System.Collections.Generic;
using CatApi.types;

namespace CatImplementations.objects.primitives
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