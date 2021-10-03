using CatApi.modules;

namespace CatImplementations.ast.nodes.modules
{
    public class FunctionDefinitionNode : IModuleObjectNode
    {
        public FunctionBodyNode Body { get; }
        public AccessModifier AccessModifier { get; private set; } = AccessModifier.Private;
        public string Name { get; }
        public string ReturnType { get; }
        public (string argName, string typeName)[] Args { get; }

        public FunctionDefinitionNode(FunctionSignatureNode signature, FunctionBodyNode body)
        {
            Body = body;
            Name = signature.Name;
            ReturnType = signature.ReturnType;
            Args = signature.Args;
        }

        public void SetAccessModifier(AccessModifier value)
        {
            AccessModifier = value;
        }
    }
}