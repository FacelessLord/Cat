using System.Collections.Generic;
using Cat.data.types.api;

namespace Cat.data.properties
{
    public class PropertyMeta
    {
        public bool Callable { get; }
        public IReadOnlyList<IDataType> ArgTypes { get; }

        public PropertyMeta()
        {
            ArgTypes = System.Array.Empty<IDataType>();
        }

        public PropertyMeta(bool callable)
        {
            Callable = callable;
            ArgTypes = System.Array.Empty<IDataType>();
        }

        public PropertyMeta(bool callable, IReadOnlyList<IDataType> argTypes)
        {
            Callable = callable;
            ArgTypes = argTypes;
        }
    }
}