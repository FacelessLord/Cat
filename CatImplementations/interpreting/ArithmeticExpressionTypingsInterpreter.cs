using System;
using CatApi.interpreting;
using CatApi.types;
using CatImplementations.ast.nodes;
using CatImplementations.ast.nodes.arithmetics;
using CatImplementations.exceptions;
using CatImplementations.interpreting.exceptions;
using CatImplementations.typings;
using CatImplementations.typings.primitives;

namespace CatImplementations.interpreting
{
    public class ArithmeticExpressionTypingsInterpreter
    {
        private readonly ITypeStorage _typeStorage;

        public ArithmeticExpressionTypingsInterpreter(ITypeStorage typeStorage)
        {
            _typeStorage = typeStorage;
        }

        public IDataType Interpret(INode node, IInterpreter<IDataType> mainInterpreter)
        {
            return node switch
            {
                ArithmeticBinaryOperationNode abon => Interpret(abon, mainInterpreter),
                ArithmeticUnaryOperationNode auon => Interpret(auon, mainInterpreter),
                _ => throw new ArgumentException(
                    $"{nameof(ArithmeticExpressionTypingsInterpreter)} is designed only to process Arithmetic nodes")
            };
        }

        private IDataType Interpret(ArithmeticBinaryOperationNode abon, IInterpreter<IDataType> mainInterpreter)
        {
            switch (abon.Operation)
            {
                case ArithmeticOperation.Is:
                    return _typeStorage[Primitives.Bool];
                case ArithmeticOperation.As:
                {
                    var typeName = ((IdNode) abon.B).IdToken;
                    var valueType = mainInterpreter.Interpret(abon.A);

                    var targetType = _typeStorage[typeName];

                    if (_typeStorage.IsTypeAssignableFrom(targetType, valueType))
                        return targetType;

                    throw new CatTypeMismatchException(valueType, targetType);
                }
            }

            var aType = mainInterpreter.Interpret(abon.A);
            var bType = mainInterpreter.Interpret(abon.B);

            var operationMethod = PropertyHelper.GetMethodForOperation(aType, abon.Operation);

            if (operationMethod == null)
                throw new CatPropertyUndefinedException(PropertyHelper.GetMethodNameForOperation(aType, abon.Operation), aType);

            var operationType = operationMethod.Type;
            if (operationType is ICallableDataType callable)
            {
                var secondArgumentType = callable.SourceTypes[1];
                var returnType = callable.TargetType;
                if (!CastFunctionsHelper.TryGetCastMethodName(_typeStorage, bType, secondArgumentType, out var _))
                    throw new CatCastException(bType, secondArgumentType);

                return returnType;
            }

            throw new CatPropertyUndefinedException("call", operationMethod.Type);
        }

        private IDataType Interpret(ArithmeticUnaryOperationNode auon, IInterpreter<IDataType> mainInterpreter)
        {
            var aType = mainInterpreter.Interpret(auon.A);
            var operationMethod = PropertyHelper.GetMethodForOperation(aType, auon.Operation);
            if (operationMethod == null)
                throw new CatPropertyUndefinedException(PropertyHelper.GetMethodNameForOperation(aType, auon.Operation), aType);
            
            var operationType = operationMethod.Type;
            if (operationType is ICallableDataType callable)
            {
                var returnType = callable.TargetType;

                return returnType;
            }

            throw new CatPropertyUndefinedException("call", operationMethod.Type);
        }
    }
}