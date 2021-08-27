using Cat.data.objects.api;
using Cat.data.objects.primitives;

namespace Cat.interpret.storage
{
    public interface IDataStorage
    {
        public IDataObject GetDataObjectByRef(IRef @ref);
        public void SetDataObjectAtRef(IRef @ref, IDataObject obj);
        public IStorageInfo GetStorageInfoForRef(IRef @ref);
        
        public IRef StoreObject(IDataObject obj);
        public void FreeObjectAtRef(IRef @ref);
    }
}