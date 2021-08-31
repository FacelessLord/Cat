using System;
using Cat.ast;
using Cat.ast.nodes;
using Cat.ast.rules;
using Cat.data.types;
using Cat.data.types.api;
using Cat.data.types.primitives;
using Cat.interpret;
using Cat.lexing;
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
        public Func<IRule, string, INode> Parse;
        public Kernel Kernel;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Kernel = new Kernel();
            Kernel.Register<TypeStorage>().AsSingleton<ITypeStorage>();
            Kernel.Register<TypingsStorage>().As<ITypingsStorage>();
            Kernel.Register<TypingsInterpreter>().As<IInterpreter<IDataType>>();
            Kernel.Register<Lexer>().As<ILexer>();
            Kernel.Register<AstBuilder>().As<IAstBuilder>();
            Kernel.Register<Func<IRule, string, INode>>(resolver => (rule, code) => 
                    resolver.Resolve<IAstBuilder>().TryBuild(rule, AstBuilder.OnError, 
                        resolver.Resolve<ILexer>().ParseCode(code)))
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
    }
}