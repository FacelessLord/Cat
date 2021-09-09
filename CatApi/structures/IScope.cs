namespace CatApi.structures
{
    public interface IScope
    {
        public string Name { get; }
        public IScope ParentScope { get; }
        public IVariable FindVariable(string name);
        public void CreateVariable(IVariable variable);
    }
}