using System;
using CatApi.interpreting;
using CatApi.lexing;
using CatApi.logger;
using CatApi.types;
using CatCollections;
using CatDi.di;
using CatImplementations.ast.rules;
using CatImplementations.interpreting;
using CatImplementations.lexing;
using CatImplementations.logging;
using CatImplementations.typings;
using NUnit.Framework;

namespace CatInterpretTests
{
    public class ModuleTests
    {
        private Kernel _kernel; 
        private Func<string, INode> Parse;
        
        [OneTimeSetUp]
        public void OneSetup()
        {
            _kernel = new Kernel();
            _kernel.Register<Logger>().AsSingleton<ILogger>();
            _kernel.Register<TypeStorage>().AsSingleton<ITypeStorage>();
            _kernel.Register<TypingsInterpreter>().As<IInterpreter<IDataType>>();
            _kernel.Register<ArithmeticExpressionTypingsInterpreter>().As<ArithmeticExpressionTypingsInterpreter>();
            _kernel.Register<Lexer>().As<ILexer>();
            _kernel.Register<Func<string, INode>>(resolver => text =>
                    Rules.Module.Read(new BufferedEnumerable<IToken>(resolver.Resolve<ILexer>()
                        .ParseCode("test.cat", text.Split("\n")))))
                .As<Func<IRule, string, INode>>();
        }
        
        [SetUp]
        public void Setup()
        {
            Parse = _kernel.Resolve<Func<string, INode>>();
        }

        [Test]
        public void ParsesModule()
        {
            var moduleText =
                "module a.b\n" +
                "import b.c\n" +
                "import c.d\n\n" +
                "\n" +
                "public class F {\n" +
                "   public function f()" +
                "}\n";
        }
    }
}