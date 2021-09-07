using System.IO;
using Cat.data.types.api;
using Cat.interpret;
using CatAst.api;
using CatAst.rules;
using CatLexing;
using CatLexing.tokens;
using CatModules.api;

namespace CatModules
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
            var module = Rules.Module.Read(new BufferedEnumerable<Token>(tokens));

            return ModuleInterpreter.Interpret(module);
        }
    }
}