using System;
using CatApi.ast.nodes;
using CatApi.ast.nodes.arithmetics;
using CatApi.exceptions;
using CatApi.structures;
using CatApi.structures.properties;
using CatApi.structures.variables;
using CatApi.types;
using CatApi.types.containers;
using CatApi.types.dataTypes;
using CatApi.types.primitives;

namespace CatApi.interpreting
{
    public class TypingsInterpreter : IInterpreter<IDataType>
    {
        private readonly TypeStorage _typeStorage;
        private readonly IScope _scope;
        private readonly ArithmeticExpressionTypingsInterpreter _arithmeticInterpreter;

        public TypingsInterpreter(TypeStorage typeStorage, IScope scope,
            ArithmeticExpressionTypingsInterpreter arithmeticInterpreter)
        {
            _typeStorage = typeStorage;
            _scope = scope;
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
            return _typeStorage[PrimitivesNames.Number];
        }

        public IDataType Interpret(StringNode node)
        {
            return _typeStorage[PrimitivesNames.String];
        }

        public IDataType Interpret(BoolNode node)
        {
            return _typeStorage[PrimitivesNames.Bool];
        }

        public IDataType Interpret(VariableStatementNode node)
        {
            var variableName = ((IdNode) node.Id).IdToken;
            var varTypeNode = node.Type;

            var expressionType = node.Expression == null ? null : Interpret(node.Expression);

            if (varTypeNode == null)
            {
                if (expressionType == null)
                {
                    throw new CatVariableTypeUnknownException();
                }

                var exprTypedvariable = new Variable(variableName, AccessRight.Rwc);
                exprTypedvariable.AssignType(expressionType);
                _scope.CreateVariable(exprTypedvariable);
                return expressionType;
            }

            var varType = _typeStorage[((IdNode) node.Type).IdToken];
            if (_scope.VariableExistsInCurrentScope(variableName))
            {
                throw new CatVariableRedeclarationException(_scope, variableName);
            }

            var explicitlyTypedVariable = new Variable(variableName, AccessRight.Rwc);

            if (expressionType != null && !_typeStorage.IsTypeAssignableFrom(varType, expressionType))
                throw new CatTypeMismatchException(expressionType, varType);

            explicitlyTypedVariable.AssignType(varType);
            _scope.CreateVariable(explicitlyTypedVariable);

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