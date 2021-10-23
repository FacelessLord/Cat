using CatApi.ast.nodes.arithmetics;
using CatApi.structures;
using CatApi.types.dataTypes;

namespace CatApi.types
{
    public static class PropertyHelper
    {
        public static IDataProperty GetMethodForOperation(IDataType sourceDataType, ArithmeticOperation operation)
        {
            var operationPropertyName = GetMethodNameForOperation(sourceDataType, operation);
            if (sourceDataType.HasProperty(operationPropertyName))
            {
                return sourceDataType.GetProperty(operationPropertyName);
            }

            return null;
        }

        public static string GetMethodNameForOperation(IDataType sourceDataType, ArithmeticOperation operation)
        {
            return $"{OperationHelper.GetOperationName(operation)}";
        }
    }
}