using System;
using System.IO;
using System.Linq;
using CatApi.interpreting;
using CatApi.lexing;
using CatApi.logger;
using CatApi.types;
using CatApi.types.containers;
using CatApi.types.dataTypes;
using CatDi.di;

namespace Cat
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = ConfigureKernel();
            var lexer = kernel.Resolve<ILexer>();
            // var parser = new AstBuilder();

            var codeFile = "./code.ct";
            var code = File.ReadLines(codeFile).ToArray();

            var tokens = lexer.ParseCode(codeFile, code).ToList();
            var logger = (Logger) kernel.Resolve<ILogger>();
            var loggerMessages = string.Join("\n", logger.Channels.SelectMany(p => p.Value.Select(s => $"[{p.Key}] {s}")));
            Console.WriteLine(loggerMessages);
            // var node = parser.TryBuild(Rules.Expression, AstBuilder.OnError, tokens);

            //todo add json serializer for nodes
            var a = 0;
        }

        static Kernel ConfigureKernel()
        {
            var kernel = new Kernel();

            kernel.Register<Logger>().AsSingleton<ILogger>();
            kernel.Register<TypeStorage>().AsSingleton<TypeStorage>();
            kernel.Register<TypingsInterpreter>().As<IInterpreter<IDataType>>();
            kernel.Register<ArithmeticExpressionTypingsInterpreter>().As<ArithmeticExpressionTypingsInterpreter>();
            kernel.Register<Lexer>().As<ILexer>();

            return kernel;
        }
    }
}