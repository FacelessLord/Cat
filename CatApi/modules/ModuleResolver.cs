using System.Collections.Generic;
using System.IO;
using CatApi.ast.rules;
using CatApi.exceptions;
using CatApi.lexing;
using CatApi.logger;
using CatApi.utils;

namespace CatApi.modules
{
    public class ModuleResolver : IModuleResolver
    {
        private readonly ILogger _logger;
        private readonly IModuleMap _moduleMap;
        private ILexer Lexer { get; }
        private IModuleInterpreter ModuleInterpreter { get; }

        private Dictionary<string, ResolutionState> _resolutionStates = new();

        public ModuleResolver(ILexer lexer, ILogger logger, IModuleInterpreter moduleInterpreter, IModuleMap moduleMap)
        {
            _logger = logger;
            _moduleMap = moduleMap;
            Lexer = lexer;
            ModuleInterpreter = moduleInterpreter;
        }

        public IModule ResolveModule(string requestSourcePath, string name)
        {
            if (_resolutionStates[name] == ResolutionState.Resolved)
                return _moduleMap.GetModule(name);
            if (_resolutionStates[name] == ResolutionState.Resolving)
                throw new CatCyclicImportException(name);
            _resolutionStates[name] = ResolutionState.Resolving;
            var module = ReadModuleFromFile(requestSourcePath, name);
            _resolutionStates[name] = ResolutionState.Resolved;

            if (module.Name != name)
                _logger.Log("Warn", "ModuleResolver", $"Module \"{module.Name}\" is defined in file named \"{name}\"");
            return module;
        }

        public IModule ReadModuleFromFile(string path, string name)
        {
            var file = File.ReadAllLines(path);

            var tokens = Lexer.ParseCode(path, file);
            var module = Rules.Module.Read(new BufferedEnumerable<IToken>(tokens));

            return ModuleInterpreter.Interpret(module, path, state => _resolutionStates[name] = state, ResolveModule);
        }
    }
}