using System.Linq;
using CatApi.interpreting;
using CatApi.modules;

namespace CatImplementations.ast.nodes.modules
{
    public class ConstructorNode : IModuleObjectNode
    {
        public (string argName, string typeName)[] Args { get; }
        public FunctionBodyNode Body { get; }

        public ConstructorNode(FunctionArgumentListNode node, FunctionBodyNode functionBodyNode)
        {
            Args = node.Arguments.Select(a => (a.Id, a.TypeName)).ToArray();
            Body = functionBodyNode;
        }

        public AccessModifier AccessModifier { get; private set; }
        public string Name { get; }

        public void SetAccessModifier(AccessModifier value)
        {
            AccessModifier = value;
        }
    }
}