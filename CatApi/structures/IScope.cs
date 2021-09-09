namespace CatApi.structures
{
    public interface IScope
    {
        public string Name { get; }
        public IScope ParentScope { get; }
        public IVariable GetVariable(string name);
        public void CreateVariable(IVariable variable);
        public bool VariableExists(string name);
        public bool VariableExistsInCurrentScope(string name);
        public string GetScopeName();
    }
}