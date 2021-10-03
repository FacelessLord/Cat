using CatApi.interpreting;

namespace CatApi.modules
{
    public interface IModuleInterpreter
    {
        public IModule Interpret(INode node, string modulePath);
    }
}