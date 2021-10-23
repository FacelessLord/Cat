using System;
using System.Collections.Generic;
using CatApi.ast.rules;
using CatApi.exceptions;
using CatApi.interpreting;
using CatApi.lexing;
using CatApi.logger;
using CatApi.objects;
using CatApi.structures;
using CatApi.structures.properties;
using CatApi.types;
using CatApi.types.containers;
using CatApi.types.dataTypes;
using CatApi.types.primitives;
using CatApi.utils;
using CatDi.di;
using FluentAssertions;
using NUnit.Framework;

namespace CatTests
{
    [TestFixture]
    public class TypingsInterpreterTests
    {
        public IInterpreter<IDataType> Interpreter;
        public TypeStorage TypeStorage;
        public Func<IRule, string, INode> Parse;
        public Kernel Kernel;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Kernel = Main.CreateTestKernel();
        }

        [SetUp]
        public void Setup()
        {
            Interpreter = Kernel.Resolve<IInterpreter<IDataType>>();
            TypeStorage = Kernel.Resolve<TypeStorage>();
            Parse = Kernel.Resolve<Func<IRule, string, INode>>();
        }

        [TearDown]
        public void TearDown()
        {
            TypeStorage.ClearTypes();
        }

        [Test]
        public void Interprets_NumberNode_As_NumberType()
        {
            var node = Parse(Rules.Literal, "124");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[PrimitivesNames.Number]);
        }

        [Test]
        public void Interprets_StringNode_As_StringType()
        {
            var node = Parse(Rules.Literal, "\"124\"");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[PrimitivesNames.String]);
        }

        [Test]
        public void Interprets_BoolNode_As_BoolType()
        {
            var node = Parse(Rules.Literal, "true");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[PrimitivesNames.Bool]);
        }

        [Test]
        public void Interprets_Typed_VariableNode_WithoutValue()
        {
            var node = Parse(Rules.LetVarStatement, "let a : system.Bool");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[PrimitivesNames.Bool]);
        }

        [Test]
        public void Interprets_Typed_VariableNode_WithValue()
        {
            var node = Parse(Rules.LetVarStatement, "let a : system.Bool = true");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[PrimitivesNames.Bool]);
        }

        [Test]
        public void Interprets_ObjectTyped_VariableNode_WithValue_OfSubtype()
        {
            var node = Parse(Rules.LetVarStatement, "let a : system.Object = true");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[PrimitivesNames.Object]);
        }

        [Test]
        public void Interprets_BoolTyped_VariableNode_WithValue_OfNotRealSubtype()
        {
            var node = Parse(Rules.LetVarStatement, "let a : system.Bool = 23");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[PrimitivesNames.Bool]);
        }

        [Test]
        public void Interprets_VarBoolToObject()
        {
            var node = Parse(Rules.LetVarStatement, "let a : system.Object = let b : system.Bool");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[PrimitivesNames.Object]);
        }

        [Test]
        public void Interprets_TypeCast_As_TargetType()
        {
            var node = Parse(Rules.Expression, "244 as system.Bool");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[PrimitivesNames.Bool]);
        }

        [Test]
        public void Interprets_Variable_TypeCast_As_TargetType()
        {
            var node = Parse(Rules.LetVarStatement, "let a = 244");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[PrimitivesNames.Number]);
        }

        [Test]
        public void Throws_When_Variable_CantBeCastTo_TargetType()
        {
            var uncastableType = new UncastableDataType();
            TypeStorage.RegisterType(uncastableType);
            var node = Parse(Rules.Expression, "244 as test.out");
            Func<IDataType> typeResolving = () => Interpreter.Interpret(node);

            typeResolving.Should().Throw<CatTypeMismatchException>();
        }

        [Test]
        public void Interprets_VariableToVariable_TypeCast_As_TargetType()
        {
            var node = Parse(Rules.Expression, "let b = let a = 244 as system.Bool");
            var resolvedType = Interpreter.Interpret(node);

            resolvedType.Should().Be(TypeStorage[PrimitivesNames.Bool]);
        }
    }

    class UncastableDataType : DataType
    {
        public UncastableDataType() : base("test.out")
        {
            Properties.Add(new DataProperty(new TypeBox("test.out"), "testProp"));
        }

        public override HashSet<IDataProperty> Properties { get; }

        public override void PopulateObject(IDataObject dataObject, params IDataObject[] args)
        {
            throw new NotImplementedException();
        }
    }
}