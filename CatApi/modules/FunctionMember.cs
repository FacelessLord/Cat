using CatApi.ast.nodes.modules;
using CatApi.types;

namespace CatApi.modules
{
    public class FunctionMember : IMember
    {
        public FunctionMember(string name, AccessModifier accessModifier, (string argName, TypeBox argType)[] args, TypeBox returnType, FunctionBodyNode body)
        {
            Name = name;
            AccessModifier = accessModifier;
            Args = args;
            ReturnType = returnType;
            Body = body;
        }

        public string Name { get; }
        public AccessModifier AccessModifier { get; }
        public (string argName, TypeBox argType)[] Args { get; }
        public TypeBox ReturnType { get; }
        public FunctionBodyNode Body { get; }
    }
}