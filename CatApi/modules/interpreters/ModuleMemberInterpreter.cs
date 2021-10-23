using System;
using System.Linq;
using CatApi.ast.nodes.modules;
using CatApi.interpreting;
using CatApi.types;

namespace CatApi.modules.interpreters
{
    public class ModuleMemberInterpreter : IInterpreter<IMember>
    {
        public IMember Interpret(INode node)
        {
            return node switch
            {
                //this interpreter just collects module without evaluation or typechecking 
                ClassDefinitionNode def => Interpret(def),
                FunctionDefinitionNode def => Interpret(def),
                FieldDefinitionNode def => Interpret(def),
                _ => throw new NotImplementedException(
                    $"Type {node.GetType().Name} is not interpretable by module member interpreter")
            };
        }

        public IMember InterpretDefinition(FunctionDefinitionNode def, AccessModifier accessModifier)
        {
            return new FunctionMember(def.Name, accessModifier,
                def.Args.Select(p => (p.argName, new TypeBox(p.typeName))).ToArray(),
                new TypeBox(def.ReturnType), def.Body);
        }

        public IMember InterpretDefinition(FieldDefinitionNode def, AccessModifier accessModifier)
        {
            return new FieldMember(def.Name, accessModifier, new TypeBox(def.Type), def.Value);
        }

        public IMember Interpret(IModuleObjectNode member)
        {
            return member switch
            {
                FunctionDefinitionNode func => InterpretDefinition(func, member.AccessModifier),
                ClassDefinitionNode clazz => Interpret(clazz),
                FieldDefinitionNode field => InterpretDefinition(field, member.AccessModifier),
                _ => throw new ArgumentOutOfRangeException(nameof(member), member, null)
            };
        }

        public IMember Interpret(ClassDefinitionNode member)
        {
            var members = member.Members.Select(Interpret);
            return new ClassMember(member.Name, member.AccessModifier, members);
        }
    }
}