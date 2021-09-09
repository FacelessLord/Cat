using System;
using CatApi.interpreting;
using CatApi.types;
using CatImplementations.ast.nodes;
using CatImplementations.ast.nodes.arithmetics;
using CatImplementations.interpreting.exceptions;
using CatImplementations.typings.primitives;

namespace CatImplementations.interpreting
{
    public class TypingsInterpreter : IInterpreter<IDataType>
    {
        private readonly ITypeStorage _typeStorage;
        private readonly ArithmeticExpressionTypingsInterpreter _arithmeticInterpreter;

        public TypingsInterpreter(ITypeStorage typeStorage, ITypingsStorage typings,
            ArithmeticExpressionTypingsInterpreter arithmeticInterpreter)
        {
            _typeStorage = typeStorage;
            _typings = typings;
            _arithmeticInterpreter = arithmeticInterpreter;
        }

        public IDataType Interpret(INode node)
        {
            return node switch
            {
                NumberNode n => Interpret(n),
                StringNode n => Interpret(n),
                BoolNode n => Interpret(n),
                VariableStatementNode n => Interpret(n),
                ArithmeticBinaryOperationNode n => Interpret(n),
                _ => throw new NotImplementedException(
                    $"Type {node.GetType().Name} is not interpretable by typing interpreter")
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

        public IDataType Interpret(ArithmeticBinaryOperationNode abon)
        {
            return _arithmeticInterpreter.Interpret(abon, this);
        }

        public IDataType Interpret(ArithmeticUnaryOperationNode auon)
        {
            return _arithmeticInterpreter.Interpret(auon, this);
        }
    }
}