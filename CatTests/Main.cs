using System;
using System.Collections.Generic;
using CatApi.bindings;
using CatApi.interpreting;
using CatApi.lexing;
using CatApi.logger;
using CatApi.modules;
using CatApi.modules.interpreters;
using CatApi.types.containers;
using CatApi.types.dataTypes;
using CatApi.utils;
using CatDi.di;

namespace CatTests
{
    public class Main
    {
        public static Kernel CreateTestKernel()
        {
            var kernel = new Kernel();
            kernel.Register<Logger>().AsSingleton<ILogger>();
            kernel.Register(r => r.FindAll<AbstractMethodBinding>()).AsSingleton<IEnumerable<AbstractMethodBinding>>();
            kernel.Register<PrimitivesMethodsBindings>().AsSingleton<PrimitivesMethodsBindings>();
            kernel.Register<TypeStorage>().AsSingleton<TypeStorage>();
            kernel.Register<TypeCreator>().As<TypeCreator>();
            kernel.Register<TypingsInterpreter>().As<IInterpreter<IDataType>>();
            kernel.Register<ArithmeticExpressionTypingsInterpreter>().As<ArithmeticExpressionTypingsInterpreter>();
            kernel.Register<Lexer>().As<ILexer>();
            kernel.Register<Func<IRule, string, INode>>(resolver => (rule, text) =>
                    rule.Read(new BufferedEnumerable<IToken>(resolver.Resolve<ILexer>()
                        .ParseCode("test.cat", text.Split("\n")))))
                .As<Func<IRule, string, INode>>();
            kernel.Register<ModuleMemberInterpreter>().As<IInterpreter<IMember>>();
            kernel.Register<ModuleInterpreter>().As<IModuleInterpreter>();
            return kernel;
        }
    }
}