using System;
using Cat.data.objects.api;
using Cat.data.properties;
using Cat.data.types;
using Cat.data.types.api;
using Cat.data.types.primitives;
using Cat.interpret;
using Cat.interpret.exceptions;
using CatAst;
using CatAst.api;
using CatAst.rules;
using CatDi.di;
using CatLexing;
using CatLexing.tokens;
using FluentAssertions;
using NUnit.Framework;

namespace CatInterpretTests
{
    [TestFixture]
    public class TypingsInterpreterTests
    {
        public IInterpreter<IDataType> Interpreter;
        public ITypeStorage TypeStorage;
        public Func<IRule, string, INode> Parse;
        public Kernel Kernel;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Kernel = new Kernel();
            Kernel.Register<Logger>().AsSingleton<ILogger>();
            Kernel.Register<TypeStorage>().AsSingleton<ITypeStorage>();
            Kernel.Register<TypingsStorage>().As<ITypingsStorage>();
            Kernel.Register<TypingsInterpreter>().As<IInterpreter<IDataType>>();
            Kernel.Register<ArithmeticExpressionTypingsInterpreter>().As<ArithmeticExpressionTypingsInterpreter>();
            Kernel.Register<Lexer>().As<ILexer>();
            Kernel.Register<Func<IRule, string, INode>>(resolver => (rule, text) =>
                    rule.Read(new BufferedEnumerable<Token>(resolver.Resolve<ILexer>().ParseCode("test.cat",text.Split("\n")))))
                .As<Func<IRule, string, INode>>();
        }

        [SetUp]
        public void Setup()
        {
            Interpreter = Kernel.Resolve<IInterpreter<IDataType>>();
            TypeStorage = Kernel.Resolve<ITypeStorage>();
            Parse = Kernel.Resolve<Func<IRule, string, INode>>();
        }

        [Test]
        public void Interprets_NumberNode_As_NumberType()
        {
            var node = Parse(Rules.Literal, "124");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[Primitives.Number]);
        }

        [Test]
        public void Interprets_StringNode_As_StringType()
        {
            var node = Parse(Rules.Literal, "\"124\"");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[Primitives.String]);
        }

        [Test]
        public void Interprets_BoolNode_As_BoolType()
        {
            var node = Parse(Rules.Literal, "true");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[Primitives.Bool]);
        }

        [Test]
        public void Interprets_Typed_VariableNode_WithoutValue()
        {
            var node = Parse(Rules.LetVarStatement, "let a : system.Bool");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[Primitives.Bool]);
        }

        [Test]
        public void Interprets_Typed_VariableNode_WithValue()
        {
            var node = Parse(Rules.LetVarStatement, "let a : system.Bool = true");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[Primitives.Bool]);
        }

        [Test]
        public void Interprets_ObjectTyped_VariableNode_WithValue_OfSubtype()
        {
            var node = Parse(Rules.LetVarStatement, "let a : system.Object = true");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[Primitives.Object]);
        }

        [Test]
        public void Interprets_BoolTyped_VariableNode_WithValue_OfNotRealSubtype()
        {
            var node = Parse(Rules.LetVarStatement, "let a : system.Bool = 23");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[Primitives.Bool]);
        }

        [Test]
        public void Interprets_VarBoolToObject()
        {
            var node = Parse(Rules.LetVarStatement, "let a : system.Object = let b : system.Bool");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[Primitives.Object]);
        }

        [Test]
        public void Interprets_TypeCast_As_TargetType()
        {
            var node = Parse(Rules.Expression, "244 as system.Bool");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[Primitives.Bool]);
        }

        [Test]
        public void Interprets_Variable_TypeCast_As_TargetType()
        {
            var node = Parse(Rules.LetVarStatement, "let a = 244");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[Primitives.Number]);
        }

        [Test]
        public void Throws_When_Variable_CantBeCastTo_TargetType()
        {
            TypeStorage.GetOrCreateType("test.out", () => new UncastableDataType());
            var node = Parse(Rules.Expression, "244 as test.out");
            Func<IDataType> typeResolving = () => Interpreter.Interpret(node);

            typeResolving.Should().Throw<CatTypeMismatchException>();
        }
        // [Test]
        // public void Interprets_VariableToVariable_TypeCast_As_TargetType()
        // {
        //     var node = Parse(Rules.Pipeline, "let b = let a = 244 as system.Bool");
        //     var resolvedType = Interpreter.Interpret(node);
        //
        //     resolvedType.Should().Be(TypeStorage[Primitives.Bool]);
        // }
    }

    class UncastableDataType : DataType
    {
        public UncastableDataType() : base("out", "test.out")
        {
            Properties.Add(new DataProperty(this, "testProp"));
        }

        public override void PopulateObject(IDataObject dataObject)
        {
            throw new NotImplementedException();
        }

        public override IDataObject CreateInstance(params object[] args)
        {
            throw new NotImplementedException();
        }

        public override IDataObject NewInstance(params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}