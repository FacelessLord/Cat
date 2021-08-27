using Cat.data.objects.api;

namespace Cat.interpret.storage
{
    public interface IRef
    {
        public IDataObject AsDataObject();

        public bool IsWeak();
        public int AsMemoryAddress();
    }
}