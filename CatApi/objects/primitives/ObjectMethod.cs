using System.Collections.Generic;
using CatApi.types;

namespace CatApi.objects.primitives
{
    public abstract class ObjectMethod : CallableDataObject
    {
        protected ObjectMethod(TypeBox function, string methodName) : base(function)
        {
            Name = methodName;
            Properties = new Dictionary<string, IDataObject>();
        }

        public string Name { get; }
    }
}