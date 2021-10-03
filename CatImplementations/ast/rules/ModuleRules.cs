using System;
using CatApi.interpreting;
using CatApi.lexing;
using CatApi.modules;
using CatImplementations.ast.nodes;
using CatImplementations.ast.nodes.modules;
using CatImplementations.lexing.tokens;
using static CatImplementations.lexing.tokens.TokenTypes;

namespace CatImplementations.ast.rules
{
    public static class ModuleRules
    {
        public static Func<ITokenType, IRule> Token = Rules.Token;
        private static Func<INode[], INode> IdentityCollector = Rules.IdentityCollector;

        public static IRule ModuleDeclaration = Rule.Named(nameof(ModuleDeclaration))
            .With(Chain.StartWith(Token(TokenTypes.Module))
                .Then(Rules.TypeName)
                .CollectBy(nodes => new ModuleDeclarationNode(nodes[1] as IdNode)));

        public static IRule ModuleImport = Rule.Named(nameof(ModuleImport))
            .With(Chain.StartWith(Token(Import))
                .Then(Rules.TypeName)
                .CollectBy(nodes => new ModuleImportNode(nodes[1] as IdNode)));

        public static IRule ModuleImports = Rule.Named(nameof(ModuleImports))
            .With(_ => Chain.StartWith(ModuleImport)
                .Then(_)
                .CollectBy(nodes => new ModuleImportListNode(nodes)))
            .With(Chain.StartWith(ModuleImport)
                .CollectBy(IdentityCollector));

        public static IRule FunctionArgs = Rule.Named(nameof(FunctionArgs))
            .With(_ => Chain.StartWith(Rules.ID)
                .Then(Token(Colon))
                .Then(Rules.TypeName)
                .Then(_)
                .CollectBy(nodes => new FunctionArgumentListNode(nodes)))
            .With(Chain.StartWith(Rules.ID)
                .Then(Token(Colon))
                .Then(Rules.TypeName)
                .CollectBy(nodes => new FunctionArgumentNode(nodes[0] as IdNode, nodes[2] as IdNode)));

        public static IRule Statement = Rule.Named(nameof(Statement))
            .With(Chain.StartWith(Rules.Expression)
                .CollectBy(IdentityCollector));

        public static IRule FunctionBody = Rule.Named(nameof(FunctionBody))
            .With(_ => Chain.StartWith(Statement)
                .Then(FunctionBody)
                .CollectBy(nodes => new FunctionBodyNode(nodes)));

        public static IRule ParenatedArgumentList = Rule.Named(nameof(ParenatedArgumentList))
            .With(Chain.StartWith(Token(LParen))
                .Then(FunctionArgs)
                .Then(Token(RParen))
                .CollectBy(nodes => nodes[1] as FunctionArgumentListNode))
            .With(Chain.StartWith(Token(LParen))
                .Then(Token(RParen))
                .CollectBy(nodes => new FunctionArgumentListNode(new INode[0])));

        public static IRule BracedFunctionBody = Rule.Named(nameof(BracedFunctionBody))
            .With(Chain.StartWith(Token(LBrace))
                .Then(FunctionBody)
                .Then(Token(RBrace))
                .CollectBy(nodes => nodes[1] as FunctionBodyNode))
            .With(Chain.StartWith(Token(LBrace))
                .Then(Token(RBrace))
                .CollectBy(nodes => new FunctionBodyNode(new INode[0])));

        public static IRule ClassConstructor = Rule.Named(nameof(ClassConstructor))
            .With(Chain.StartWith(Token(Constructor))
                .Then(ParenatedArgumentList)
                .Then(BracedFunctionBody)
                .CollectBy(nodes =>
                    new ConstructorNode(nodes[1] as FunctionArgumentListNode, nodes[5] as FunctionBodyNode)));

        public static IRule AccessModifier = Rule.Named(nameof(AccessModifier))
            .With(Chain.StartWith(Token(Public))
                .CollectBy(IdentityCollector))
            .With(Chain.StartWith(Token(Private))
                .CollectBy(IdentityCollector));

        public static IRule ClassMember = Rule.Named(nameof(ClassMember))
            .With(_ => Chain.StartWith(FunctionDefinition)
                .CollectBy(IdentityCollector))
            .With(_ => Chain.StartWith(ConstDefinition)
                .CollectBy(IdentityCollector))
            .With(_ => Chain.StartWith(ClassDefinition)
                .CollectBy(IdentityCollector))
            .With(_ => Chain.StartWith(ClassConstructor)
                .CollectBy(IdentityCollector));

        public static IRule ClassMembers = Rule.Named(nameof(ClassMembers))
            .With(_ => Chain.StartWith(ClassMember)
                .Then(_)
                .CollectBy(nodes => new ClassMemberListNode(nodes)))
            .With(Chain.StartWith(ClassMember)
                .CollectBy(nodes => new ClassMemberListNode(nodes)));

        public static IRule ClassDefinition = Rule.Named(nameof(ClassDefinition))
            .With(Chain.StartWith(Token(Class))
                .Then(Rules.ID)
                .Then(Token(LBrace))
                .Then(ClassMembers)
                .Then(Token(RBrace))
                .CollectBy(nodes => new ClassDefinitionNode(nodes[1] as IdNode, nodes[3] as ClassMemberListNode)))
            .With(Chain.StartWith(Token(Class))
                .Then(Rules.ID)
                .Then(Token(LBrace))
                .Then(Token(RBrace))
                .CollectBy(nodes =>
                    new ClassDefinitionNode(nodes[1] as IdNode, new ClassMemberListNode(new INode[0]))));


        public static IRule FunctionSignature = Rule.Named(nameof(FunctionSignature))
            .With(Chain.StartWith(Token(Function))
                .Then(Rules.ID)
                .Then(ParenatedArgumentList)
                .Then(Token(Colon))
                .Then(Rules.TypeName)
                .CollectBy(nodes => new FunctionSignatureNode(nodes[1] as IdNode, nodes[2], nodes[4] as IdNode)));

        public static IRule FunctionDefinition = Rule.Named(nameof(FunctionDefinition))
            .With(Chain.StartWith(FunctionSignature)
                .Then(BracedFunctionBody)
                .CollectBy(nodes =>
                    new FunctionDefinitionNode(nodes[0] as FunctionSignatureNode, nodes[1] as FunctionBodyNode)));

        public static IRule ConstDefinition = Rule.Named(nameof(ConstDefinition))
            .With(Chain.StartWith(Token(Const))
                .Then(Rules.ID)
                .Then(Token(Colon))
                .Then(Rules.TypeName)
                .Then(Token(Set))
                .Then(Rules.Expression)
                .CollectBy(nodes => new ConstDefinitionNode(nodes[1] as IdNode, nodes[3] as IdNode, nodes[5])))
            .With(Chain.StartWith(Token(Const))
                .Then(Rules.ID)
                .Then(Token(Colon))
                .Then(Rules.TypeName)
                .CollectBy(nodes => new ConstDefinitionNode(nodes[1] as IdNode, nodes[3] as IdNode, null)));

        public static IRule ModuleObject = Rule.Named(nameof(ModuleObject))
            .With(Chain.StartWith(ClassDefinition)
                .CollectBy(IdentityCollector))
            .With(Chain.StartWith(FunctionDefinition)
                .CollectBy(IdentityCollector))
            .With(Chain.StartWith(ConstDefinition)
                .CollectBy(IdentityCollector));

        public static IRule PrefixedModuleObject = Rule.Named(nameof(ModuleObject))
            .With(Chain.StartWith(AccessModifier)
                .Then(ModuleObject)
                .CollectBy(nodes =>
                {
                    var accessModifier = AccessModifiersHelper.FromString(((TokenNode) nodes[0]).Token.Value);
                    var node = (IModuleObjectNode) nodes[1];
                    node.SetAccessModifier(accessModifier);
                    return node;
                }))
            .With(Chain.StartWith(ModuleObject)
                .CollectBy(IdentityCollector));

        public static IRule ModuleInternals = Rule.Named(nameof(ModuleInternals))
            .With(_ => Chain.StartWith(PrefixedModuleObject)
                .Then(_)
                .CollectBy(nodes => new ModuleObjectListNode(nodes)))
            .With(Chain.StartWith(PrefixedModuleObject)
                .CollectBy(nodes => new ModuleObjectListNode(nodes)));
        
        public static IRule Module = Rule.Named(nameof(Module))
            .With(Chain.StartWith(ModuleDeclaration)
                .Then(ModuleImports)
                .Then(ModuleInternals)
                .CollectBy(nodes => new ModuleNode(nodes)));

        //todo tests for all rules
        //todo cross-tests for rules
    }
}