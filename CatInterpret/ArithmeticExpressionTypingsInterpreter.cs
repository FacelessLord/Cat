using System;
using Cat.data.types;
using Cat.data.types.api;
using Cat.interpret.exceptions;
using CatAst;
using CatAst.nodes;
using CatAst.nodes.arithmetics;

namespace Cat.interpret
{
    public class ArithmeticExpressionTypingsInterpreter
    {
        private readonly ITypeStorage _typeStorage;
        private readonly ITypingsStorage _typings;

        public ArithmeticExpressionTypingsInterpreter(ITypeStorage typeStorage,
            ITypingsStorage typings)
        {
            _typeStorage = typeStorage;
            _typings = typings;
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

        public IDataType Interpret(ArithmeticBinaryOperationNode abon, IInterpreter<IDataType> mainInterpreter)
        {
            if (abon.Operation == ArithmeticOperation.Is || abon.Operation == ArithmeticOperation.As)
            {
                var typeName = ((IdNode) abon.B).IdToken;
                var valueType = mainInterpreter.Interpret(abon.A);

                var targetType = _typeStorage[typeName];

                if (_typeStorage.IsTypeAssignableFrom(targetType, valueType))
                    return targetType;

                throw new CatTypeMismatchException(valueType, targetType);
            }

            var aType = mainInterpreter.Interpret(abon.A);
            var bType = mainInterpreter.Interpret(abon.B);
            return null;
        }

        public IDataType Interpret(ArithmeticUnaryOperationNode auon, IInterpreter<IDataType> mainInterpreter)
        {
            return null;
        }
    }
}