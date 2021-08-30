using Cat.ast.nodes;
using Cat.data.types;
using Cat.data.types.api;
using Cat.data.types.primitives;
using Cat.interpret;
using Cat.lexing.tokens;
using CatDi.di;
using FluentAssertions;
using NUnit.Framework;

namespace CatInterpretTests
{
    public class TypingsInterpreterTests
    {
        public IInterpreter<IDataType> Interpreter;
        public ITypeStorage TypeStorage;
        public Kernel Kernel;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Kernel = new Kernel();
            Kernel.Register<TypeStorage>().AsSingleton<ITypeStorage>();
            Kernel.Register<TypingsInterpreter>().As<IInterpreter<IDataType>>();
        }
        
        [SetUp]
        public void Setup()
        {
            Interpreter = Kernel.Resolve<IInterpreter<IDataType>>();
            TypeStorage = Kernel.Resolve<ITypeStorage>();
        }

        [Test]
        public void Interprets_NumberNode_As_NumberType()
        {
            var node = new NumberNode(new TokenNode(new Token(TokenTypes.Number, "125")));
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[Primitives.Number]);
        }
        
        [Test]
        public void Interprets_StringNode_As_StringType()
        {
            var node = new StringNode(new TokenNode(new Token(TokenTypes.String, "125")));
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[Primitives.String]);
        }
        
    }
}