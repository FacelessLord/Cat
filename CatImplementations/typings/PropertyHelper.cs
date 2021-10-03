using CatApi.structures;
using CatApi.types;
using CatImplementations.ast.nodes.arithmetics;

namespace CatImplementations.typings
{
    public static class PropertyHelper
    {
        public static IDataProperty GetMethodForOperation(IDataType sourceType, ArithmeticOperation operation)
        {
            var operationPropertyName = GetMethodNameForOperation(sourceType, operation);
            if (sourceType.HasProperty(operationPropertyName))
            {
                return sourceType.GetProperty(operationPropertyName);
            }

            return null;
        }

        public static string GetMethodNameForOperation(IDataType sourceType, ArithmeticOperation operation)
        {
            return $"{OperationHelper.GetOperationName(operation)}";
        }
    }
}