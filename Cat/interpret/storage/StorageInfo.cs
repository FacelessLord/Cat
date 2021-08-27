using System;
using System.Collections.Generic;
using Cat.data.types;

namespace Cat.interpret.storage
{
    public class StorageInfo : IStorageInfo
    {
        private readonly TypeStorage _types;
        private readonly Action<Action> _onLastRefDestroyed;
        public int RefCount { get; private set; }
        public int StrongRefCount { get; private set; }
        public HashSet<IRef> Refs { get; private set; } = new HashSet<IRef>();
        public IRef MainRef { get; }
        public bool Disposed { get; private set; }

        public StorageInfo(TypeStorage types, IRef mainRef, Action<Action> onLastRefDestroyed)
        {
            _types = types;
            _onLastRefDestroyed = onLastRefDestroyed;
            MainRef = mainRef;
            RefCount = 1;
            StrongRefCount = mainRef.IsWeak() ? 0 : 1;
            Refs.Add(MainRef);
        }

        public IRef CreateRef()
        {
            var @ref = new Ref(_types, MainRef.AsMemoryAddress());
            Refs.Add(@ref);
            RefCount++;
            StrongRefCount++;
            return @ref;
        }

        public IRef CreateWeakRef()
        {
            var @ref = new Ref(_types, MainRef.AsMemoryAddress(), true);
            Refs.Add(@ref);
            RefCount++;
            return @ref;
        }

        public bool DestroyRef(IRef @ref)
        {
            if (!Refs.Remove(@ref)) return false;

            RefCount--;
            if (!@ref.IsWeak())
                StrongRefCount--;
            if (StrongRefCount == 0)
            {
                _onLastRefDestroyed(() =>
                {
                    Refs.Clear();
                    Disposed = true;
                });
            }

            return true;
        }
    }
}