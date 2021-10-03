using System;
using System.Linq;
using CatApi.interpreting;
using CatApi.modules;
using CatApi.types;
using CatImplementations.ast.nodes.modules;

namespace CatImplementations.modules
{
    public class ModuleInterpreter : IModuleInterpreter
    {
        private readonly ITypeStorage _typeStorage;
        private readonly IModuleResolver _moduleResolver;
        private readonly IInterpreter<IMember> _moduleMemberInterpreter;

        public ModuleInterpreter(ITypeStorage typeStorage, ModuleResolver moduleResolver, IInterpreter<IMember> moduleMemberInterpreter)
        {
            _typeStorage = typeStorage;
            _moduleResolver = moduleResolver;
            _moduleMemberInterpreter = moduleMemberInterpreter;
        }

        public IModule Interpret(INode node, string modulePath)
        {
            return node switch
            {
                //this interpreter just collects module without evaluation or typechecking 
                ModuleNode module => Interpret(module, modulePath),
                _ => throw new NotImplementedException(
                    $"Type {node.GetType().Name} is not interpretable by module interpreter")
            };
        }

        public IModule Interpret(ModuleNode module, string modulePath)
        {
            var moduleMembers = module.Objects
                .Select(_moduleMemberInterpreter.Interpret)
                .ToArray();
            var importedModules = module.Imports
                .Select(name => _moduleResolver.ResolveModuleByName(".", name))
                .ToArray();
            return new Module(module.Name, modulePath, importedModules, moduleMembers);
        }
    }
}