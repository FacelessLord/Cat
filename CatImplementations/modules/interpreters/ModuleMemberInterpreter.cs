using System;
using System.Linq;
using CatApi.interpreting;
using CatApi.modules;
using CatImplementations.ast;
using CatImplementations.ast.nodes.modules;

namespace CatImplementations.modules
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
                ConstDefinitionNode def => Interpret(def),
                _ => throw new NotImplementedException(
                    $"Type {node.GetType().Name} is not interpretable by module member interpreter")
            };
        }

        public IMember InterpretDefinition(FunctionDefinitionNode def, AccessModifier accessModifier)
        {
            return new FunctionMember(def.Name, accessModifier);
        }

        public IMember InterpretDefinition(ConstDefinitionNode def, AccessModifier accessModifier)
        {
            return new ConstMember(def.Name, accessModifier);
        }

        public IMember Interpret(IModuleObjectNode member)
        {
            return member switch
            {
                FunctionDefinitionNode func => InterpretDefinition(func, member.AccessModifier),
                ClassDefinitionNode clazz => Interpret(clazz),
                ConstDefinitionNode conzt => InterpretDefinition(conzt, member.AccessModifier),
                _ => throw new ArgumentOutOfRangeException(nameof(member), member, null)
            };
        }

        public IMember Interpret(ClassDefinitionNode member)
        {
            var members = member.Members.Members.Select(Interpret);
            return new ClassMember(member.Name, member.AccessModifier, members);
        }
    }
}