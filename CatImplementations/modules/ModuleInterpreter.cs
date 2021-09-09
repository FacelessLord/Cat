using System;
using CatApi.interpreting;
using CatApi.modules;

namespace CatImplementations.modules
{
    public class ModuleInterpreter : IInterpreter<IModule>
    {
        public IModule Interpret(INode node)
        {
            return node switch
            {
                
                _ => throw new NotImplementedException(
                    $"Type {node.GetType().Name} is not interpretable by module interpreter")
            };
        }
    }
}