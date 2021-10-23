using System;
using System.Linq;
using CatApi.ast.nodes.modules;
using CatApi.interpreting;
using CatApi.types;
using CatApi.types.containers;
using CatApi.utils;

namespace CatApi.modules.interpreters
{
    public class ModuleInterpreter : IModuleInterpreter
    {
        private readonly IInterpreter<IMember> _moduleMemberInterpreter;
        private readonly TypeStorage _typeStorage;
        private readonly TypeCreator _typeCreator;

        public ModuleInterpreter(IInterpreter<IMember> moduleMemberInterpreter, TypeCreator typeCreator, TypeStorage typeStorage)
        {
            _moduleMemberInterpreter = moduleMemberInterpreter;
            _typeCreator = typeCreator;
            _typeStorage = typeStorage;
        }

        public IModule Interpret(INode node, string modulePath, Action<ResolutionState> setResolutionState,
            Func<string, string, IModule> resolveModule)
        {
            return node switch
            {
                //this interpreter just collects module without evaluation or typechecking 
                ModuleNode module => Interpret(module, modulePath, setResolutionState, resolveModule),
                _ => throw new NotImplementedException(
                    $"Type {node.GetType().Name} is not interpretable by module interpreter")
            };
        }

        public IModule Interpret(ModuleNode module, string modulePath, Action<ResolutionState> setResolutionState,
            Func<string, string, IModule> resolveModule)
        {
            var moduleMembers = module.Objects
                .Select(_moduleMemberInterpreter.Interpret)
                .ToArray();
            setResolutionState(ResolutionState.ResolvingDependencies);
            var importedModules = module.Imports
                .Select(name => resolveModule(".", name))
                .ToArray();
            moduleMembers
                .Where(m => m is ClassMember)
                .Select(_typeCreator.CreateType)
                .ForEach(_typeStorage.RegisterType);
            return new Module(module.Name, modulePath, importedModules, moduleMembers);
        }
    }
}