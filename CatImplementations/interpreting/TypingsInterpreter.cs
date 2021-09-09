using System;
using CatApi.interpreting;
using CatApi.structures;
using CatApi.types;
using CatImplementations.ast.nodes;
using CatImplementations.ast.nodes.arithmetics;
using CatImplementations.interpreting.exceptions;
using CatImplementations.structures.properties;
using CatImplementations.structures.variables;
using CatImplementations.typings.primitives;

namespace CatImplementations.interpreting
{
    public class TypingsInterpreter : IInterpreter<IDataType>
    {
        private readonly ITypeStorage _typeStorage;
        private readonly IScope _scope;
        private readonly ArithmeticExpressionTypingsInterpreter _arithmeticInterpreter;

        public TypingsInterpreter(ITypeStorage typeStorage, IScope scope,
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