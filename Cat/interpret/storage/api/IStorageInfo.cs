using System.Collections;
using System.Collections.Generic;

namespace Cat.interpret.storage
{
    public interface IStorageInfo
    {
        public int RefCount { get; }
        public int StrongRefCount { get; }
        public HashSet<IRef> Refs { get; }
        public IRef MainRef { get; }

        public bool Disposed { get; }

        public IRef CreateRef();
        public IRef CreateWeakRef();
        
        public bool DestroyRef(IRef @ref);
    }
}