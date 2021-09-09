using System;
using System.Linq;
using CatAst.api;
using CatAst.nodes;
using CatAst.nodes.modules;
using CatLexing.tokens;
using static CatLexing.tokens.TokenTypes;

namespace CatAst.rules
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

        public static IRule ClassDefinition = Rule.Named(nameof(ClassDefinition))
            .With(Chain.StartWith(Token(Class))
                .Then(Rules.ID)
                .Then(Token(LBrace))
                //todo class internals
                .Then(Token(RBrace))
                .CollectBy(nodes => new ClassDefinitionNode(nodes[1] as IdNode)));

        public static IRule FunctionDefinition = Rule.Named(nameof(FunctionDefinition))
            .With(Chain.StartWith(Token(Function))
                .Then(Rules.ID)
                .Then(Token(LParen))
                //todo func args
                .Then(Token(RParen))
                .Then(Token(Colon))
                .Then(Rules.TypeName)
                .Then(Token(LBrace))
                //todo func internals
                .Then(Token(RBrace))
                .CollectBy(nodes => new FunctionDefinitionNode(nodes[1] as IdNode, nodes[5] as IdNode)));

        public static IRule ConstDefinition = Rule.Named(nameof(ConstDefinition))
            .With(Chain.StartWith(Token(Const))
                .Then(Rules.ID)
                .Then(Token(Colon))
                .Then(Rules.TypeName)
                //todo const value
                .CollectBy(nodes => new ConstDefinitionNode(nodes[1] as IdNode, nodes[3] as IdNode)));

        public static IRule ModuleObject = Rule.Named(nameof(ModuleObject))
            .With(Chain.StartWith(ClassDefinition)
                .CollectBy(IdentityCollector))
            .With(Chain.StartWith(FunctionDefinition)
                .CollectBy(IdentityCollector))
            .With(Chain.StartWith(ConstDefinition)
                .CollectBy(IdentityCollector));

        public static IRule PrefixedModuleObject = Rule.Named(nameof(ModuleObject))
            .With(Chain.StartWith(Token(Export))
                .Then(ModuleObject)
                .CollectBy(nodes =>
                {
                    var node = (IModuleObjectNode) nodes[1];
                    node.SetExported(true);
                    return node;
                }))
            .With(Chain.StartWith(ModuleObject)
                .CollectBy(IdentityCollector));

        public static IRule ModuleInternals = Rule.Named(nameof(ModuleInternals))
            .With(_ => Chain.StartWith(PrefixedModuleObject)
                .Then(_)
                .CollectBy(nodes => new ModuleObjectListNode(nodes)))
            .With(Chain.StartWith(PrefixedModuleObject)
                .CollectBy(IdentityCollector));


        public static IRule Module = Rule.Named(nameof(Module))
            .With(Chain.StartWith(ModuleDeclaration)
                .Then(ModuleImports)
                .Then(ModuleInternals)
                .CollectBy(nodes => new ModuleNode(nodes)));

        //todo tests for all rules
        //todo cross-tests for rules
    }
}