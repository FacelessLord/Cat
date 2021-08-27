using Cat.data.objects.api;
using Cat.data.types;
using Cat.data.types.primitives;

namespace Cat.interpret.storage
{
    public class Ref : IRef
    {
        private readonly TypeStorage _types;
        private readonly int _address;
        private readonly bool _isWeak;

        public Ref(TypeStorage types, int address, bool isWeak=false)
        {
            _types = types;
            _address = address;
            _isWeak = isWeak;
        }

        public IDataObject AsDataObject()
        {
            return _types[Primitives.Number].CreateInstance(_address);
        }

        public bool IsWeak()
        {
            return _isWeak;
        }

        public int AsMemoryAddress()
        {
            return _address;
        }
    }
}