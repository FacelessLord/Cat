using System;
using System.Collections.Generic;
using Cat.data.objects.api;
using Cat.data.objects.primitives;
using Cat.data.types;
using Cat.interpret.storage.exceptions;

namespace Cat.interpret.storage
{
    public class DataStorage : IDataStorage
    {
        private readonly TypeStorage _types;

        public DataStorage(TypeStorage types)
        {
            _types = types;
        }

        public List<(IStorageInfo info, IDataObject obj)> Memory = new();

        public IDataObject GetDataObjectByRef(IRef @ref)
        {
            return AccessObject(@ref, (_, obj, __) => obj);
        }

        public void SetDataObjectAtRef(IRef @ref, IDataObject obj)
        {
            AccessObject(@ref, (info, _, address) => Memory[address] = (info, obj));
        }

        public IStorageInfo GetStorageInfoForRef(IRef @ref)
        {
            return AccessObject(@ref, (info, _, __) => info);
        }

        public IRef StoreObject(IDataObject obj)
        {
            var mainRef = new Ref(_types, Memory.Count, false);
            var info = new StorageInfo(_types, mainRef, disposeInfo =>
            {
                FreeObjectAtRef(mainRef);
                disposeInfo();
            });

            Memory[Memory.Count] = (info, obj);

            return mainRef;
        }

        public void FreeObjectAtRef(IRef @ref)
        {
            AccessObject(@ref, (info, _, address) => Memory[address] = (info, null));
        }

        public T AccessObject<T>(IRef @ref, Func<IStorageInfo, IDataObject, int, T> onAccess)
        {
            var address = @ref.AsMemoryAddress();
            if (address < 0 || address >= Memory.Count) throw new CatIllegalMemoryAccessException(@ref);
            
            var (info, obj) = Memory[address];
            if (info.Disposed)
                throw new CatAccessToDisposedObjectException(@ref);
            return onAccess(info, obj, address);

        }
    }
}