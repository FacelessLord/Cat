using System;
using CatApi.ast.rules;
using CatApi.interpreting;
using CatApi.modules;
using CatDi.di;
using FluentAssertions;
using NUnit.Framework;

namespace CatTests
{
    public class ModuleTests
    {
        private Kernel _kernel;
        private IModuleInterpreter ModuleInterpreter;
        private Func<IRule, string, INode> Parse;

        [OneTimeSetUp]
        public void OneSetup()
        {
            _kernel = Main.CreateTestKernel();
        }

        [SetUp]
        public void Setup()
        {
            Parse = _kernel.Resolve<Func<IRule, string, INode>>();
            ModuleInterpreter = _kernel.Resolve<IModuleInterpreter>();
        }

        [Test]
        public void ParsesFunctionSignature()
        {
            var funcSig = "function h() : system.Bool";
            var funcSigNode = Parse(ModuleRules.FunctionSignature, funcSig);

            funcSigNode.Should().NotBeNull();
        }

        [Test]
        public void ParsesEmptyFunctionBody()
        {
            var funcSig = "{}";
            var funcSigNode = Parse(ModuleRules.BracedFunctionBody, funcSig);

            funcSigNode.Should().NotBeNull();
        }

        [Test]
        public void ParsesFunction()
        {
            var func = "function g() : system.Bool{let a:system.Number = 1}";
            var funcNode = Parse(ModuleRules.FunctionDefinition, func);

            funcNode.Should().NotBeNull();
        }

        [Test]
        public void ParsesFunctionWithEmptyBody()
        {
            var func = "function f() : system.Bool{}";
            var funcNode = Parse(ModuleRules.FunctionDefinition, func);

            funcNode.Should().NotBeNull();
        }

        [Test]
        public void ParsesFunctionBody()
        {
            var funcSig = "{let a:system.Number = 1}";
            var funcSigNode = Parse(ModuleRules.BracedFunctionBody, funcSig);

            funcSigNode.Should().NotBeNull();
        }

        [Test]
        public void ParsesClassWithFunctionInside()
        {
            var classF = "class H {\n" +
                         "   public function f() : system.Bool {}\n" +
                         "}";
            var classFNode = Parse(ModuleRules.ClassDefinition, classF);

            classFNode.Should().NotBeNull();
        }

        [Test]
        public void ParsesEmptyClass()
        {
            var classF = "class F {\n" +
                         "}";
            var classFNode = Parse(ModuleRules.ClassDefinition, classF);

            classFNode.Should().NotBeNull();
        }

        [Test]
        public void ParsesModule()
        {
            var moduleText =
                "module a.b\n\n" +
                "public class G {\n" +
                "   public function f() : system.Bool {}\n" +
                "}\n" +
                "private const A:system.Number = 3.141592653589793";
            var moduleNode = Parse(Rules.Module, moduleText);

            var module = ModuleInterpreter.Interpret(moduleNode, "path", s => {}, (a, b) => null);
            module.Should().NotBeNull();
        }
    }
}