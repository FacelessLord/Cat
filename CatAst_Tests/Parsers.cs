// using System;
// using System.Linq;
// using Cat.ast;
// using Cat.ast.nodes;
// using Cat.ast.rules;
// using Cat.lexing;
// using Cat.lexing.tokens;
// using FluentAssertions;
// using NUnit.Framework;
//
// namespace CatTests
// {
//     [TestFixture]
//     public class Parsers
//     {
//         private static Lazy<Lexer> lexer = new(() => new Lexer());
//         private static Lazy<AstBuilder> parser = new(() => new AstBuilder());
//         
//         [Test]
//         public void ParserParses_MostSimplePipeline()
//         {
//             var code = "2";
//             var tokens = lexer.Value.ParseCode(code);
//             var node = parser.Value.TryBuild(Rules.FunctionBody, AstBuilder.OnError, tokens);
//
//             node.Should().BeOfType<NumberNode>();
//         }
//         
//         [Test]
//         public void ParserParses_SimplePipeline()
//         {
//             var code = "\"1\" | 124 & '12'";
//             var tokens = lexer.Value.ParseCode(code);
//             var node = parser.Value.TryBuild(Rules.FunctionBody, AstBuilder.OnError, tokens);
//
//             node.Should().BeOfType<PipelineOrNode>();
//             var pipelineOrNode = (PipelineOrNode) node;
//             pipelineOrNode.A.Should().BeOfType<StringNode>();
//             pipelineOrNode.B.Should().BeOfType<PipelineAndNode>();
//             var pipelineAndNode = (PipelineAndNode) pipelineOrNode.B;
//             pipelineAndNode.A.Should().BeOfType<NumberNode>();
//             pipelineAndNode.B.Should().BeOfType<StringNode>();
//         }
//
//         [Test]
//         public void ParserParses_FlatListLiteral()
//         {
//             var code = "[123123, 21, '3',\"23\", testVar]";
//             var tokens = lexer.Value.ParseCode(code).ToList();
//             var node = parser.Value.TryBuild(Rules.Literal, AstBuilder.OnError, tokens);
//
//             node.Should().BeOfType<ListNode>();
//             var listNode = (ListNode) node;
//             listNode.ListInternals.Should().BeOfType<ExpressionListNode>();
//             var expListNode = (ExpressionListNode) listNode.ListInternals;
//             expListNode.Nodes[0].Should().BeOfType<NumberNode>();
//             expListNode.Nodes[1].Should().BeOfType<NumberNode>();
//             expListNode.Nodes[2].Should().BeOfType<StringNode>();
//             expListNode.Nodes[3].Should().BeOfType<StringNode>();
//             expListNode.Nodes[4].Should().BeOfType<IdNode>();
//         }
//
//         [Test]
//         public void ParserParses_NestedListLiteral()
//         {
//             var code = "[123123, [1, 4], testVar]";
//             var tokens = lexer.Value.ParseCode(code).ToList();
//             var node = parser.Value.TryBuild(Rules.Literal, AstBuilder.OnError, tokens);
//
//             node.Should().BeOfType<ListNode>();
//             var list = (ListNode) node;
//             list.ListInternals.Should().BeOfType<ExpressionListNode>();
//             var expList = list.ListInternals;
//             expList.Nodes[0].Should().BeOfType<NumberNode>();
//             expList.Nodes[1].Should().BeOfType<ListNode>();
//             expList.Nodes[2].Should().BeOfType<IdNode>();
//
//             var secondList = (ListNode) expList.Nodes[1];
//             secondList.ListInternals.Should().BeOfType<ExpressionListNode>();
//             var secondExpList = secondList.ListInternals;
//
//             secondExpList.Nodes[0].Should().BeOfType<NumberNode>();
//             secondExpList.Nodes[1].Should().BeOfType<NumberNode>();
//         }
//
//         [Test]
//         public void ParserParses_FlatObjectLiteral()
//         {
//             var code = "{ a : 1, b:\":\", c:c}";
//             var tokens = lexer.Value.ParseCode(code).ToList();
//             var node = parser.Value.TryBuild(Rules.Literal, AstBuilder.OnError, tokens);
//
//             node.Should().BeOfType<ObjectLiteralNode>();
//             var objectNode = (ObjectLiteralNode) node;
//             var valuePairListNode = objectNode.PairsList;
//             valuePairListNode.Nodes[0].Id.Should().BeOfType<IdNode>();
//             valuePairListNode.Nodes[0].Expression.Should().BeOfType<NumberNode>();
//             valuePairListNode.Nodes[1].Id.Should().BeOfType<IdNode>();
//             valuePairListNode.Nodes[1].Expression.Should().BeOfType<StringNode>();
//             valuePairListNode.Nodes[2].Id.Should().BeOfType<IdNode>();
//             valuePairListNode.Nodes[2].Expression.Should().BeOfType<IdNode>();
//         }
//
//         [Test]
//         public void ParserParses_NestedObjectLiteral()
//         {
//             var code = "{c:{c:{f:'F'}}}";
//
//             var tokens = lexer.Value.ParseCode(code).ToList();
//             var node = parser.Value.TryBuild(Rules.Literal, AstBuilder.OnError, tokens);
//
//             node.Should().BeOfType<ObjectLiteralNode>();
//             var objectNode = (ObjectLiteralNode) node;
//             var valuePairListNode = objectNode.PairsList;
//             valuePairListNode.Nodes[0].Id.Should().BeOfType<IdNode>();
//             valuePairListNode.Nodes[0].Expression.Should().BeOfType<ObjectLiteralNode>();
//             var objLit1 = (ObjectLiteralNode) valuePairListNode.Nodes[0].Expression;
//             objLit1.PairsList.Nodes[0].Id.Should().BeOfType<IdNode>();
//             objLit1.PairsList.Nodes[0].Expression.Should().BeOfType<ObjectLiteralNode>();
//             var objLit2 = (ObjectLiteralNode) objLit1.PairsList.Nodes[0].Expression;
//             objLit2.PairsList.Nodes[0].Id.Should().BeOfType<IdNode>();
//             objLit2.PairsList.Nodes[0].Expression.Should().BeOfType<StringNode>();
//         }
//
//         [Test]
//         public void ParserParses_VariableDefinitionWithType()
//         {
//             var code = "let c : A";
//
//             var tokens = lexer.Value.ParseCode(code).ToList();
//             var node = parser.Value.TryBuild(Rules.LetVarStatement, AstBuilder.OnError, tokens);
//
//             node.Should().BeOfType<VariableStatementNode>();
//             var variableNode = (VariableStatementNode) node;
//             variableNode.Id.Should().BeOfType<IdNode>();
//             variableNode.ExpressionNode.Should().BeNull();
//         }
//         
//         [Test]
//         public void ParserParses_Id()
//         {
//             var code = "a";
//
//             var tokens = lexer.Value.ParseCode(code).ToList();
//             var node = parser.Value.TryBuild(Rules.Token(TokenTypes.Id), AstBuilder.OnError, tokens);
//
//             node.Should().BeOfType<TokenNode>();
//         }
//         [Test]
//         public void ParserParses_Id_As_TypeName()
//         {
//             var code = "A";
//
//             var tokens = lexer.Value.ParseCode(code).ToList();
//             var node = parser.Value.TryBuild(Rules.TypeName, AstBuilder.OnError, tokens);
//
//             node.Should().BeOfType<IdNode>();
//             var variableNode = (IdNode) node;
//             variableNode.IdToken.Should().Be(code);
//         }
//         [Test]
//         public void ParserParses_TypeName()
//         {
//             var code = "A.B.C";
//
//             var tokens = lexer.Value.ParseCode(code).ToList();
//             var node = parser.Value.TryBuild(Rules.TypeName, AstBuilder.OnError, tokens);
//
//             node.Should().BeOfType<IdNode>();
//             var variableNode = (IdNode) node;
//             variableNode.IdToken.Should().Be(code);
//         }
//         
//         [Test]
//         public void ParserParses_TypeCast()
//         {
//             var code = "(5) as system.Bool";
//
//             var tokens = lexer.Value.ParseCode(code).ToList();
//             var node = parser.Value.TryBuild(Rules.Expression, AstBuilder.OnError, tokens);
//
//             node.Should().BeOfType<TypeCastNode>();
//             var variableNode = (TypeCastNode) node;
//             variableNode.Type.Should().BeOfType<IdNode>();
//             variableNode.Value.Should().BeOfType<NumberNode>();
//         }
//         [Test]
//         public void ParserParses_NestedTypeCast()
//         {
//             var code = "((5) as system.Object) as system.Bool";
//             //todo Create a way to enable left-end recursion (at least triggered by some token after rule)
//             // like if we have "expression AS ..." (dots for all next tokens) and we want to
//             // check for typecasting expression (expression AS typename) we can check if next token is "AS"
//             // and then run ICompletingRule on expressionNode and token list.
//             // if rule succeeds then we use standard-interface node collector with expressionNode and
//             // all next found rule nodes to create wrapper node and then we return wrapper instead of expressionNode.
//             // But if completing rule won't succeed we check next completing rules.
//             // if all completing rules are unsuccessful we return expressionNode.
//             // This allows us to have postfix operators without strong pain in ass,
//             // but restricts us to create lots of left-recursive rules
//             var tokens = lexer.Value.ParseCode(code).ToList();
//             var node = parser.Value.TryBuild(Rules.Expression, AstBuilder.OnError, tokens);
//
//             node.Should().BeOfType<TypeCastNode>();
//             var variableNode = (TypeCastNode) node;
//             variableNode.Type.Should().BeOfType<IdNode>();
//             variableNode.Value.Should().BeOfType<TypeCastNode>();
//             var subVariableNode = (TypeCastNode) variableNode.Value;
//             subVariableNode.Type.Should().BeOfType<IdNode>();
//             subVariableNode.Value.Should().BeOfType<NumberNode>();
//         }
//         
//         [Test]
//         public void ParserParses_VariableDefinitionWithValueSet()
//         {
//             var code = "let c = 2";
//
//             var tokens = lexer.Value.ParseCode(code).ToList();
//             var node = parser.Value.TryBuild(Rules.Expression, AstBuilder.OnError, tokens);
//
//             node.Should().BeOfType<VariableStatementNode>();
//             var variableNode = (VariableStatementNode) node;
//             variableNode.Id.Should().BeOfType<IdNode>();
//             variableNode.ExpressionNode.Should().BeOfType<NumberNode>();
//         }
//         
//         [Test]
//         public void ParserParses_VariableDefinitionWithValueSetByPipeline()
//         {
//             var code = "( let c = 2 )";
//
//             var tokens = lexer.Value.ParseCode(code).ToList();
//             var node = parser.Value.TryBuild(Rules.Expression, AstBuilder.OnError, tokens);
//
//             node.Should().BeOfType<VariableStatementNode>();
//             var variableNode = (VariableStatementNode) node;
//             variableNode.Id.Should().BeOfType<IdNode>();
//             variableNode.ExpressionNode.Should().BeOfType<NumberNode>();
//         }
//         
//         [Test]
//         public void ParserParses_HardThing_VariableDefinitionWithValueSetByObjectInPipeline()
//         {
//             var code = "let c = ( { a: { L: let a = ({d: 3}) }, d: [{i: \"2\"}] } | [0] )";
//
//             var tokens = lexer.Value.ParseCode(code).ToList();
//             var node = parser.Value.TryBuild(Rules.LetVarStatement, AstBuilder.OnError, tokens);
//
//             node.Should().BeOfType<VariableStatementNode>();
//         }
//         
//         [Test]
//         public void ParserParses_ObjectWithParentheses()
//         {
//             var code = "{ L: ( 2 ) }";
//
//             var tokens = lexer.Value.ParseCode(code).ToList();
//             var node = parser.Value.TryBuild(Rules.Pipeline, AstBuilder.OnError, tokens);
//
//             node.Should().BeOfType<ObjectLiteralNode>();
//         }
//     }
// }