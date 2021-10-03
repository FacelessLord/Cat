using System.IO;
using CatApi.interpreting;
using CatApi.lexing;
using CatApi.modules;
using CatCollections;
using CatImplementations.ast.rules;

namespace CatImplementations.modules
{
    public class ModuleResolver : IModuleResolver
    {
        public ILexer Lexer { get; }
        public IInterpreter<IModule> ModuleInterpreter { get; }

        public ModuleResolver(ILexer lexer, IInterpreter<IModule> moduleInterpreter)
        {
            Lexer = lexer;
            ModuleInterpreter = moduleInterpreter;
        }
        
        public IModule ResolveModuleByPath(string requestSourcePath, string path)
        {
            throw new System.NotImplementedException();
        }

        public IModule ResolveModuleByName(string requestSourcePath, string name)
        {
            throw new System.NotImplementedException();
        }

        public IModule ReadModuleFromFile(string path)
        {
            var file = File.ReadAllLines(path);

            var tokens = Lexer.ParseCode(path, file);
            var module = Rules.Module.Read(new BufferedEnumerable<IToken>(tokens));

            return ModuleInterpreter.Interpret(module);
        }
    }
}