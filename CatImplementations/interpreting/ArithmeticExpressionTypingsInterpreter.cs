using System;
using CatApi.interpreting;
using CatApi.types;
using CatImplementations.ast.nodes;
using CatImplementations.ast.nodes.arithmetics;
using CatImplementations.interpreting.exceptions;
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
            return null;
        }

        private IDataType Interpret(ArithmeticUnaryOperationNode auon, IInterpreter<IDataType> mainInterpreter)
        {
            return null;
        }
    }
}