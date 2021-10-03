using System.Linq;
using CatApi.interpreting;

namespace CatImplementations.ast.nodes.modules
{
    public class FunctionSignatureNode : INode
    {
        public string ReturnType { get; }
        public string Name { get; }
        public (string argName, string typeName)[] Args { get; }

        public FunctionSignatureNode(IdNode name, INode args, IdNode returnType)
        {
            Name = name.IdToken;
            ReturnType = returnType.IdToken;

            switch (args)
            {
                case FunctionArgumentListNode fargs:
                    Args = fargs.Arguments.Select(a => (a.Id, a.TypeName)).ToArray();
                    return;
                case FunctionArgumentNode farg:
                    Args = new[] { (farg.Id, farg.TypeName) };
                    return;
            }
        }
    }
}