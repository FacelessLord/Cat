using System;
using Cat.data.types;
using Cat.data.types.api;
using Cat.data.types.primitives;
using Cat.interpret.exceptions;
using CatAst;
using CatAst.nodes;

namespace Cat.interpret
{
    public class TypingsInterpreter : IInterpreter<IDataType>
    {
        private readonly ITypeStorage _typeStorage;
        private readonly ITypingsStorage _typings;

        public TypingsInterpreter(ITypeStorage typeStorage, ITypingsStorage typings)
        {
            _typeStorage = typeStorage;
            _typings = typings;
        }

        public IDataType Interpret(INode node)
        {
            return node switch
            {
                NumberNode n => Interpret(n),
                StringNode n => Interpret(n),
                BoolNode n => Interpret(n),
                VariableStatementNode n => Interpret(n),
                _ => throw new NotImplementedException($"Type {node.GetType().Name} is not interpretable")
            };
        }

        public IDataType Interpret(NumberNode node)
        {
            return _typeStorage[Primitives.Number];
        }

        public IDataType Interpret(StringNode node)
        {
            return _typeStorage[Primitives.String];
        }

        public IDataType Interpret(BoolNode node)
        {
            return _typeStorage[Primitives.Bool];
        }

        public IDataType Interpret(VariableStatementNode node)
        {
            var variableName = ((IdNode) node.Id).IdToken;
            var varTypeNode = node.Type;

            var expressionType = node.Expression == null ? null : Interpret(node.Expression);

            if (varTypeNode == null)
            {
                if (_typings.HasVariable(variableName))
                {
                    throw new CatVariableRedeclarationException(variableName);
                }

                if (expressionType == null)
                {
                    throw new CatVariableTypeUnknownException();
                }

                _typings.SetVariableType(variableName, expressionType);
                return expressionType;
            }

            var varType = _typeStorage[((IdNode) node.Type).IdToken];
            if (_typings.HasVariable(variableName))
            {
                throw new CatVariableRedeclarationException(variableName);
            }

            _typings.SetVariableType(variableName, varType);

            if (expressionType != null && !_typeStorage.IsTypeAssignableFrom(varType, expressionType))
                throw new CatTypeMismatchException(expressionType, varType);

            return varType;
        }
    }
}