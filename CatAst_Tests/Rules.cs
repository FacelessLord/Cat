using System;
using System.Linq;
using CatApi.interpreting;
using CatApi.lexing;
using CatCollections;
using CatImplementations.ast.nodes;
using CatImplementations.ast.nodes.arithmetics;
using CatImplementations.ast.nodes.modules;
using CatImplementations.ast.rules;
using CatImplementations.lexing.tokens;
using FluentAssertions;
using NUnit.Framework;
using static CatImplementations.lexing.tokens.TokenTypes;

namespace CatAst_Tests
{
    [TestFixture]
    public class Tests
    {
        public static IRule Token(ITokenType type) => new TokenRule(type);

        private static Token colon = new Token(Colon, ":");
        private static Token set = new Token(Set, "=");
        private static Token equals = new Token(TokenTypes.Equals, "==");
        private static Token dot = new Token(Dot, ".");
        private static Token let = new Token(Let, "let");
        private static Token import = new Token(Import, "import");
        private static Token export = new Token(Export, "export");
        private static Token module = new Token(Module, "module");
        private static Token function = new Token(Function, "function");
        private static Token @class = new Token(Class, "class");
        private static Token @const = new Token(Const, "const");
        private static Token plus = new Token(Plus, "plus");
        private static Token minus = new Token(Minus, "minus");
        private static Token lParen = new Token(LParen, "(");
        private static Token rParen = new Token(RParen, ")");
        private static Token lBracket = new Token(LBracket, "[");
        private static Token rBracket = new Token(RBracket, "]");
        private static Token lBrace = new Token(LBrace, "{");
        private static Token rBrace = new Token(RBrace, "}");
        private static Token comma = new Token(Comma, ",");
        private static Token star = new Token(Star, "*");
        private static Token divide = new Token(Divide, "/");
        private static Token @as = new Token(As, "as");
        private static Token @is = new Token(TokenTypes.Is, "is");

        private static Token a = new Token(Id, "a");
        private static Token b = new Token(Id, "b");
        private static Token c = new Token(Id, "c");
        private static Token d = new Token(Id, "d");

        private static Token numberA = new Token(Number, "12215");
        private static Token numberB = new Token(Number, "74373");
        private static Token numberC = new Token(Number, "9125");

        private static Token stringA = new Token(TokenTypes.String, "\"1gas2215\"");
        private static Token stringB = new Token(TokenTypes.String, "\"74ajt73\"");
        private static Token stringC = new Token(TokenTypes.String, "\"qizs\"");

        private static Token boolTrue = new Token(Bool, "true");
        private static Token boolFalse = new Token(Bool, "false");

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SimpleTokenRule()
        {
            var rule = Token(Let);

            var node = rule.Read(new BufferedEnumerable<IToken>(new[] { let }));

            node.Should().BeOfType<TokenNode>();
            var tokenNode = (TokenNode) node;
            tokenNode.Token.Should().Be(let);
        }

        private class NodeList : INode
        {
            public INode[] Nodes { get; }

            public NodeList(INode[] nodes)
            {
                Nodes = nodes;
            }
        }

        [Test]
        public void LetVarTokenRule()
        {
            var rule = Rule.Named("testRule")
                .With(Chain.StartWith(Token(Let))
                    .Then(Token(Id))
                    .Then(Token(Colon))
                    .Then(Token(Id))
                    .CollectBy(nodes => new NodeList(nodes)));

            var tokens = new[] { let, a, colon, b };
            var node = rule.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<NodeList>();
            var nodeList = (NodeList) node;
            var list = nodeList.Nodes.Select((n, i) => (n, t: tokens[i])).ToList();
            list.ForEach(p => p.n.Should().BeOfType<TokenNode>());
            list.ForEach(p => ((TokenNode) p.n).Token.Should().Be(p.t));
        }

        [Test]
        public void RecursiveRule()
        {
            CompositeRule tailRule = null;

            CompositeRule rrule = null;
            rrule = Rule.Named("testRule")
                .With(_ => Chain.StartWith(_)
                    .Then(Token(Colon))
                    .Then(Token(Id))
                    .Then(Token(Dot))
                    .Then(Token(Id))
                    .CollectTailBy(nodes => new NodeList(new[] { nodes[1], nodes[3] })))
                .With(Chain.StartWith(Token(Id))
                    .Then(Token(Dot))
                    .Then(Token(Id))
                    .CollectBy(nodes => new NodeList(new[] { nodes[0], nodes[2] })));

            var mainRule = CompositeRule.FixLeftRecursion(rrule, nodes => new NodeList(nodes));

            var tokens = new[] { a, dot, b, colon, c, dot, d };
            var node = mainRule.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<NodeList>();
            var nodeList = (NodeList) node;
            var list = nodeList.Nodes.Select((n, i) => (n, t: tokens[i])).ToList();
            list.ForEach(p => p.n.Should().BeOfType<NodeList>());
            list.ForEach(p =>
                ((NodeList) p.n).Nodes.Select(n => n.Should().BeOfType<TokenNode>()).Count().Should().Be(2));
        }

        [Test]
        public void MultipleRecursiveRule()
        {
            CompositeRule tailRule = null;

            CompositeRule rrule = null;
            rrule = Rule.Named("testRule")
                .With(_ => Chain.StartWith(_)
                    .Then(Token(Colon))
                    .Then(Token(Id))
                    .Then(Token(Dot))
                    .Then(Token(Id))
                    .CollectTailBy(nodes => new NodeList(new[] { nodes[1], nodes[3] })))
                .With(_ => Chain.StartWith(_)
                    .Then(Token(Colon))
                    .Then(Token(Id))
                    .Then(Token(Minus))
                    .Then(Token(Id))
                    .CollectTailBy(nodes => new NodeList(new[] { nodes[1], nodes[3] })))
                .With(Chain.StartWith(Token(Id))
                    .Then(Token(Dot))
                    .Then(Token(Id))
                    .CollectBy(nodes => new NodeList(new[] { nodes[0], nodes[2] })))
                .With(Chain.StartWith(Token(Id))
                    .Then(Token(Plus))
                    .Then(Token(Id))
                    .CollectBy(nodes => new NodeList(new[] { nodes[0], nodes[2] })));

            var mainRule = CompositeRule.FixLeftRecursion(rrule, nodes => new NodeList(nodes));

            var tokens = new[] { a, plus, b, colon, c, minus, d, colon, a, dot, b };
            var node = mainRule.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<NodeList>();
            var nodeList = (NodeList) node;
            var list = nodeList.Nodes.Select((n, i) => (n, t: tokens[i])).ToList();
            list.ForEach(p => p.n.Should().BeOfType<NodeList>());
            list.ForEach(p =>
                ((NodeList) p.n).Nodes.Select(n => n.Should().BeOfType<TokenNode>()).Count().Should().Be(2));
        }

        [Test]
        public void TypeNameRule_Works()
        {
            var tokens = new[] { a, dot, b, dot, c, dot, d };

            var node = Rules.TypeName.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<IdNode>();

            var idNode = (IdNode) node;
            idNode.IdToken.Should().Be("a.b.c.d");
        }

        [Test]
        public void LetVarRule_Parses_IdNameWithTypeAndValue()
        {
            var tokens = new[] { let, a, colon, b, dot, c, @set, numberA };

            var node = Rules.LetVarStatement.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<VariableStatementNode>();

            var idNode = (VariableStatementNode) node;
            idNode.Id.Should().BeOfType<IdNode>();
            idNode.Type.Should().BeOfType<IdNode>();
            idNode.Expression.Should().BeOfType<NumberNode>();

            var id = (IdNode) idNode.Id;
            id.IdToken.Should().Be(a.Value);
            var type = (IdNode) idNode.Type;
            type.IdToken.Should().Be("b.c");
            var expr = (NumberNode) idNode.Expression;
            expr.NumberToken.Should().Be(numberA.Value);
        }

        [Test]
        public void LetVarRule_Parses_IdNameWithType()
        {
            var tokens = new[] { let, a, colon, b, dot, c };

            var node = Rules.LetVarStatement.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<VariableStatementNode>();

            var idNode = (VariableStatementNode) node;
            idNode.Id.Should().BeOfType<IdNode>();
            idNode.Type.Should().BeOfType<IdNode>();
            idNode.Expression.Should().BeNull();

            var id = (IdNode) idNode.Id;
            id.IdToken.Should().Be(a.Value);
            var type = (IdNode) idNode.Type;
            type.IdToken.Should().Be("b.c");
        }

        [Test]
        public void LetVarRule_Parses_IdNameWithValue()
        {
            var tokens = new[] { let, a, @set, b };

            var node = Rules.LetVarStatement.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<VariableStatementNode>();

            var idNode = (VariableStatementNode) node;
            idNode.Id.Should().BeOfType<IdNode>();
            idNode.Type.Should().BeNull();
            idNode.Expression.Should().BeOfType<IdNode>();

            var id = (IdNode) idNode.Id;
            id.IdToken.Should().Be(a.Value);
            var expr = (IdNode) idNode.Expression;
            expr.IdToken.Should().Be("b");
        }

        [Test]
        public void Literal_Parses_Number()
        {
            var tokens = new[] { numberA };

            var node = Rules.Literal.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<NumberNode>();

            var numberNode = (NumberNode) node;
            numberNode.NumberToken.Should().Be(numberA.Value);
        }

        [Test]
        public void Literal_Parses_String()
        {
            var tokens = new[] { stringA };

            var node = Rules.Literal.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<StringNode>();

            var stringNode = (StringNode) node;
            stringNode.StringToken.Should().Be(stringA.Value);
        }

        [Test]
        public void Literal_Parses_Bool()
        {
            //true
            {
                var tokens = new[] { boolTrue };
                var node = Rules.Literal.Read(new BufferedEnumerable<IToken>(tokens));

                node.Should().BeOfType<BoolNode>();

                var boolNode = (BoolNode) node;
                boolNode.BoolToken.Should().Be(boolTrue.Value);
            }

            //false
            {
                var tokens = new[] { boolFalse };
                var node = Rules.Literal.Read(new BufferedEnumerable<IToken>(tokens));

                node.Should().BeOfType<BoolNode>();

                var boolNode = (BoolNode) node;
                boolNode.BoolToken.Should().Be(boolFalse.Value);
            }
        }

        [Test]
        public void Literal_Parses_List()
        {
            var tokens = new[] { lBracket, stringB, comma, numberC, comma, boolTrue, comma, a, rBracket };

            var node = Rules.Literal.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<ListNode>();

            var listNode = (ListNode) node;

            listNode.Nodes[0].Should().BeOfType<StringNode>();
            ((StringNode) listNode.Nodes[0]).StringToken.Should().Be(stringB.Value);
            listNode.Nodes[1].Should().BeOfType<NumberNode>();
            ((NumberNode) listNode.Nodes[1]).NumberToken.Should().Be(numberC.Value);
            listNode.Nodes[2].Should().BeOfType<BoolNode>();
            ((BoolNode) listNode.Nodes[2]).BoolToken.Should().Be(boolTrue.Value);
            listNode.Nodes[3].Should().BeOfType<IdNode>();
            ((IdNode) listNode.Nodes[3]).IdToken.Should().Be(a.Value);
        }

        [Test]
        public void Literal_Parses_NestedList()
        {
            var tokens = new[]
            {
                lBracket, stringB, comma, lBrace, a, colon, lBracket, numberC, rBracket, rBrace, comma, lBracket,
                boolTrue, rBracket, comma, a, rBracket
            };

            var node = Rules.Literal.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<ListNode>();

            var listNode = (ListNode) node;

            listNode.Nodes[0].Should().BeOfType<StringNode>();
            listNode.Nodes[1].Should().BeOfType<ObjectLiteralNode>();
            ((ObjectLiteralNode) listNode.Nodes[1]).Nodes[0].Expression.Should().BeOfType<ListNode>();
            listNode.Nodes[2].Should().BeOfType<ListNode>();
            listNode.Nodes[3].Should().BeOfType<IdNode>();
        }

        [Test]
        public void Literal_Parses_Object()
        {
            var tokens = new[]
            {
                lBrace, a, colon, stringB, comma, b, colon, numberC, comma, c, colon, boolTrue, comma, d, colon, a,
                rBrace
            };

            var node = Rules.Literal.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<ObjectLiteralNode>();

            var objectNode = (ObjectLiteralNode) node;

            objectNode.Nodes[0].Expression.Should().BeOfType<StringNode>();
            ((StringNode) objectNode.Nodes[0].Expression).StringToken.Should().Be(stringB.Value);
            objectNode.Nodes[1].Expression.Should().BeOfType<NumberNode>();
            ((NumberNode) objectNode.Nodes[1].Expression).NumberToken.Should().Be(numberC.Value);
            objectNode.Nodes[2].Expression.Should().BeOfType<BoolNode>();
            ((BoolNode) objectNode.Nodes[2].Expression).BoolToken.Should().Be(boolTrue.Value);
            objectNode.Nodes[3].Expression.Should().BeOfType<IdNode>();
            ((IdNode) objectNode.Nodes[3].Expression).IdToken.Should().Be(a.Value);

            objectNode.Nodes.Select(n => n.Id.Should().BeOfType<IdNode>()).Count().Should().Be(4);
        }

        [Test]
        public void Literal_Parses_NestedObject()
        {
            var tokens = new[]
            {
                lBrace, a, colon, stringB, comma, b, colon, lBrace, b, colon, numberC, rBrace, comma, c, colon,
                lBracket, boolTrue, comma, d, rBracket,
                rBrace
            };

            var node = Rules.Literal.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<ObjectLiteralNode>();

            var objectNode = (ObjectLiteralNode) node;

            objectNode.Nodes[0].Expression.Should().BeOfType<StringNode>();
            objectNode.Nodes[1].Expression.Should().BeOfType<ObjectLiteralNode>();
            objectNode.Nodes[2].Expression.Should().BeOfType<ListNode>();

            objectNode.Nodes.Select(n => n.Id.Should().BeOfType<IdNode>()).Count().Should().Be(3);
        }

        [Test]
        public void ArithmeticRule_Parses_NumberSum()
        {
            var tokens = new[]
            {
                numberA, plus, lParen, plus, numberB, minus, minus, numberC, rParen, star, numberA, divide, numberB,
                @as, a, dot, b,
                @is, c, dot, d
            };

            var node = Rules.ArithmeticExpression.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<ArithmeticBinaryOperationNode>();

            var node1 = (ArithmeticBinaryOperationNode) node;

            node1.Operation.Should().Be(ArithmeticOperation.Plus);
            node1.A.Should().BeOfType<NumberNode>();
            (node1.A as NumberNode)!.NumberToken.Should().Be(numberA.Value);
            node1.B.Should().BeOfType<ArithmeticBinaryOperationNode>();
            var node2 = (ArithmeticBinaryOperationNode) node1.B;

            node2.Operation.Should().Be(ArithmeticOperation.Divide);
            node2.A.Should().BeOfType<ArithmeticBinaryOperationNode>();
            node2.B.Should().BeOfType<ArithmeticBinaryOperationNode>();

            var node3 = (ArithmeticBinaryOperationNode) node2.A;
            node3.Operation.Should().Be(ArithmeticOperation.Multiply);
            node3.A.Should().BeOfType<ArithmeticBinaryOperationNode>();
            node3.B.Should().BeOfType<NumberNode>();
            var node5 = (ArithmeticBinaryOperationNode) node3.A;
            node5.Operation.Should().Be(ArithmeticOperation.Minus);
            node5.A.Should().BeOfType<ArithmeticUnaryOperationNode>();
            node5.B.Should().BeOfType<ArithmeticUnaryOperationNode>();
            var node7 = ((ArithmeticUnaryOperationNode) node5.A);
            node7.Operation.Should().Be(ArithmeticOperation.Plus);
            node7.A.Should().BeOfType<NumberNode>();
            ((NumberNode) node7.A).NumberToken.Should().Be(numberB.Value);
            var node6 = ((ArithmeticUnaryOperationNode) node5.B);
            node6.Operation.Should().Be(ArithmeticOperation.Minus);
            node6.A.Should().BeOfType<NumberNode>();
            ((NumberNode) node6.A).NumberToken.Should().Be(numberC.Value);

            var node4 = (ArithmeticBinaryOperationNode) node2.B;
            node4.Operation.Should().Be(ArithmeticOperation.Is);
            node4.A.Should().BeOfType<ArithmeticBinaryOperationNode>();
            node4.B.Should().BeOfType<IdNode>();

            var node8 = (ArithmeticBinaryOperationNode) node4.A;
            node8.Operation.Should().Be(ArithmeticOperation.As);
            node8.A.Should().BeOfType<NumberNode>();
            node8.B.Should().BeOfType<IdNode>();

            var node9 = (NumberNode) node8.A;
            node9.NumberToken.Should().Be(numberB.Value);
            var node10 = (IdNode) node8.B;
            node10.IdToken.Should().Be("a.b");

            var node11 = (IdNode) node4.B;
            node11.IdToken.Should().Be("c.d");
        }

        [Test]
        public void Flatten_WorksProperly_On_NestedList()
        {
            var tokenNumberA = new TokenNode(numberA);
            var tokenPlus = new TokenNode(plus);
            var tokenNumberB = new TokenNode(numberB);
            var node = new INode[]
            {
                tokenNumberA,
                new CompositeRule.NodeList(new INode[]
                    { tokenPlus, new CompositeRule.NodeList(new INode[] { tokenNumberB }) })
            };

            var flatNodes = ArithmeticTree.Flatten(node);
            flatNodes.Count.Should().Be(3);
            flatNodes[0].Should().Be(tokenNumberA);
            flatNodes[1].Should().Be(tokenPlus);
            flatNodes[2].Should().Be(tokenNumberB);
        }

        [Test]
        public void Flatten_WorksProperly_On_SingleItemArray()
        {
            var tokenNumberA = new TokenNode(numberA);
            var node = new INode[] { tokenNumberA };

            var flatNodes = ArithmeticTree.Flatten(node);
            flatNodes.Count.Should().Be(1);
            flatNodes[0].Should().Be(tokenNumberA);
        }

        [Test]
        public void ModuleDeclarationRule_Works()
        {
            var tokens = new[]
            {
                module, a, dot, b
            };

            var node = ModuleRules.ModuleDeclaration.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<ModuleDeclarationNode>();
            var moduleDecl = (ModuleDeclarationNode) node;
            moduleDecl.ModuleName.Should().Be("a.b");
        }

        [Test]
        public void ModuleImportRule_Works()
        {
            var tokens = new[]
            {
                import, a, dot, b
            };

            var node = ModuleRules.ModuleImport.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<ModuleImportNode>();
            var importNode = (ModuleImportNode) node;
            importNode.Target.IdToken.Should().Be("a.b");
        }

        [Test]
        public void ModuleImportListRule_Works()
        {
            var tokens = new[]
            {
                import, a, dot, b,
                import, b, dot, c,
                import, c, dot, a
            };

            var node = ModuleRules.ModuleImports.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<ModuleImportListNode>();
            var importListNode = (ModuleImportListNode) node;
            importListNode.Imports.Length.Should().Be(3);
            importListNode.Imports[0].Target.IdToken.Should().Be("a.b");
            importListNode.Imports[1].Target.IdToken.Should().Be("b.c");
            importListNode.Imports[2].Target.IdToken.Should().Be("c.a");
        }

        [Test]
        public void ClassDefinitionRule_Works()
        {
            var tokens = new[]
            {
                @class, a, lBrace, rBrace
            };

            var node = ModuleRules.ClassDefinition.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<ClassDefinitionNode>();
            var classDefinition = (ClassDefinitionNode) node;
            classDefinition.Name.Should().Be("a");
            classDefinition.Exported.Should().BeFalse();
        }

        [Test]
        public void ExportClassDefinitionRule_Works()
        {
            var tokens = new[]
            {
                export, @class, a, lBrace, rBrace
            };

            var node = ModuleRules.PrefixedModuleObject.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<ClassDefinitionNode>();
            var classDefinition = (ClassDefinitionNode) node;
            classDefinition.Name.Should().Be("a");
            classDefinition.Exported.Should().BeTrue();
        }

        [Test]
        public void FunctionDefinitionRule_Works()
        {
            var tokens = new[]
            {
                function, a, lParen, rParen, colon, b, dot, c, lBrace, rBrace
            };

            var node = ModuleRules.FunctionDefinition.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<FunctionDefinitionNode>();
            var functionDefinition = (FunctionDefinitionNode) node;
            functionDefinition.Name.Should().Be("a");
            functionDefinition.ReturnType.Should().Be("b.c");
            functionDefinition.Exported.Should().BeFalse();
        }

        [Test]
        public void ExportFunctionDefinitionRule_Works()
        {
            var tokens = new[]
            {
                export, function, a, lParen, rParen, colon, b, dot, c, lBrace, rBrace
            };

            var node = ModuleRules.PrefixedModuleObject.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<FunctionDefinitionNode>();
            var functionDefinition = (FunctionDefinitionNode) node;
            functionDefinition.Name.Should().Be("a");
            functionDefinition.ReturnType.Should().Be("b.c");
            functionDefinition.Exported.Should().BeTrue();
        }

        [Test]
        public void ConstDefinitionRule_Works()
        {
            var tokens = new[]
            {
                @const, a, colon, b, dot, c
            };

            var node = ModuleRules.ConstDefinition.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<ConstDefinitionNode>();
            var constDefinition = (ConstDefinitionNode) node;
            constDefinition.Name.Should().Be("a");
            constDefinition.Type.Should().Be("b.c");
            constDefinition.Exported.Should().BeFalse();
        }

        [Test]
        public void ExportConstDefinitionRule_Works()
        {
            var tokens = new[]
            {
                export, @const, a, colon, b, dot, c
            };

            var node = ModuleRules.PrefixedModuleObject.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<ConstDefinitionNode>();
            var constDefinition = (ConstDefinitionNode) node;
            constDefinition.Name.Should().Be("a");
            constDefinition.Type.Should().Be("b.c");
            constDefinition.Exported.Should().BeTrue();
        }
        
        [Test]
        public void ModuleInternalsRule_Works()
        {
            var tokens = new[]
            {
                export, @class, a, lBrace, rBrace, @const, a, colon, b, dot, c, function, a, lParen, rParen, colon, b, dot, c, lBrace, rBrace
            };

            var node = ModuleRules.ModuleInternals.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<ModuleObjectListNode>();
            var moduleObjects = (ModuleObjectListNode) node;
            moduleObjects.Objects.Length.Should().Be(3);
            moduleObjects.Objects[0].Should().BeOfType<ClassDefinitionNode>();
            moduleObjects.Objects[1].Should().BeOfType<ConstDefinitionNode>();
            moduleObjects.Objects[2].Should().BeOfType<FunctionDefinitionNode>();

            var classDef = (ClassDefinitionNode) moduleObjects.Objects[0];
            var constDef = (ConstDefinitionNode) moduleObjects.Objects[1];
            var funcDef = (FunctionDefinitionNode) moduleObjects.Objects[2];

            classDef.Exported.Should().BeTrue();
            classDef.Name.Should().Be("a");
            constDef.Exported.Should().BeFalse();
            constDef.Name.Should().Be("a");
            constDef.Type.Should().Be("b.c");
            funcDef.Exported.Should().BeFalse();
            funcDef.Name.Should().Be("a");
            funcDef.ReturnType.Should().Be("b.c");
        }
        
        [Test]
        public void ModuleRule_Works()
        {
            var tokens = new[]
            {
                module, a, dot, b,
                import, a, dot, b,
                import, b, dot, c,
                import, c, dot, a,
                
                export, @class, a, lBrace, rBrace,
                @const, a, colon, b, dot, c,
                function, a, lParen, rParen, colon, b, dot, c, lBrace, rBrace
            };

            var node = ModuleRules.Module.Read(new BufferedEnumerable<IToken>(tokens));

            node.Should().BeOfType<ModuleNode>();
            var moduleNode = (ModuleNode) node;
            moduleNode.Name.Should().Be("a.b");
            moduleNode.Imports.Should().Equal("a.b", "b.c", "c.a");
            moduleNode.Objects.Length.Should().Be(3);

            var classDef = (ClassDefinitionNode) moduleNode.Objects[0];
            var constDef = (ConstDefinitionNode) moduleNode.Objects[1];
            var funcDef = (FunctionDefinitionNode) moduleNode.Objects[2];

            classDef.Exported.Should().BeTrue();
            classDef.Name.Should().Be("a");
            constDef.Exported.Should().BeFalse();
            constDef.Name.Should().Be("a");
            constDef.Type.Should().Be("b.c");
            funcDef.Exported.Should().BeFalse();
            funcDef.Name.Should().Be("a");
            funcDef.ReturnType.Should().Be("b.c");
        }
    }
}