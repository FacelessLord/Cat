using System;
using CatApi.ast.nodes;
using CatApi.ast.nodes.arithmetics;
using CatApi.exceptions;
using CatApi.types;
using CatApi.types.containers;
using CatApi.types.dataTypes;
using CatApi.types.primitives;

namespace CatApi.interpreting
{
    public class ArithmeticExpressionTypingsInterpreter
    {
        private readonly TypeStorage _typeStorage;

        public ArithmeticExpressionTypingsInterpreter(TypeStorage typeStorage)
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
                    return _typeStorage[PrimitivesNames.Bool];
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
                throw new CatPropertyUndefinedException(PropertyHelper.GetMethodNameForOperation(aType, abon.Operation),
                    aType);

            var operationType = _typeStorage[operationMethod.DataType];
            if (operationType is CallableDataType callable)
            {
                var secondArgumentType = _typeStorage[callable.SourceTypes[1]];
                var returnType = _typeStorage[callable.TargetDataType];
                if (!CastFunctionsHelper.TryGetCastMethodName(_typeStorage, bType, secondArgumentType, out _))
                    throw new CatCastException(bType, secondArgumentType);

                return returnType;
            }

            throw new CatPropertyUndefinedException("call", _typeStorage[operationMethod.DataType]);
        }

        private IDataType Interpret(ArithmeticUnaryOperationNode auon, IInterpreter<IDataType> mainInterpreter)
        {
            var aType = mainInterpreter.Interpret(auon.A);
            var operationMethod = PropertyHelper.GetMethodForOperation(aType, auon.Operation);
            if (operationMethod == null)
                throw new CatPropertyUndefinedException(PropertyHelper.GetMethodNameForOperation(aType, auon.Operation),
                    aType);

            var operationType = _typeStorage[operationMethod.DataType];
            if (operationType is CallableDataType callable)
            {
                var returnType = _typeStorage[callable.TargetDataType];

                return returnType;
            }

            throw new CatPropertyUndefinedException("call", _typeStorage[operationMethod.DataType]);
        }
    }
}