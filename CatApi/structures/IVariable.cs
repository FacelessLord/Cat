using CatApi.objects;

namespace CatApi.structures
{
    public interface IVariable
    {
        public int AccessRights { get; }
        public IDataObject Value { get; }
        public string Name { get; }

        public void Set(IDataObject value);
        public IDataObject Get();
        public IDataObject Call(IScope scope, IDataObject[] arguments);
    }
}