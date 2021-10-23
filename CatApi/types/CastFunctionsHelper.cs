using System.Linq;
using CatApi.ast.nodes;
using CatApi.exceptions;
using CatApi.interpreting;
using CatApi.types.containers;
using CatApi.types.dataTypes;

namespace CatApi.types
{
    public static class CastFunctionsHelper
    {
        public const string CastFunctionBaseName = "object.castTo";

        public static string CreateCastFunctionName(IDataType typeBox) => CastFunctionBaseName + typeBox.FullName;

        public static bool TryGetCastMethodName(TypeStorage typeStorage, IDataType sourceType, IDataType targetType,
            out string castMethodName)
        {
            var suitableTargetTypes = sourceType.Properties.Select(p => p.Name)
                .Where(name => name.StartsWith(CastFunctionBaseName))
                .Where(name => typeStorage
                    .IsTypeAssignableFrom(targetType, typeStorage[name[CastFunctionBaseName.Length..]]))
                .ToList();

            if (suitableTargetTypes.Count == 0)
            {
                castMethodName = null;
                return false;
            }

            castMethodName = suitableTargetTypes.First();
            return true;
        }

        public static INode AddTypeCastIfNeeded(TypeStorage typeStorage, INode node, IDataType sourceType,
            IDataType targetType)
        {
            if (typeStorage.IsTypeAssignableFrom(targetType, sourceType))
            {
                return node;
            }

            if (TryGetCastMethodName(typeStorage, sourceType, targetType, out var castMethodName))
            {
                var sourceNode = CreateTypeAccessNode(sourceType);
                sourceNode = CreatePropertyAccessNode(sourceNode, castMethodName);
                sourceNode = new ObjectCallNode(sourceNode, new ExpressionListNode(node));

                return sourceNode;
            }

            throw new CatCastException(sourceType, targetType);
        }

        public static INode CreatePropertyAccessNode(INode source, string propertyName)
        {
            return new PropertyAccessNode(source, new IdNode(propertyName));
        }

        public static INode ObjectCallNode(INode source, INode[] args)
        {
            return new ObjectCallNode(source, new ExpressionListNode(args));
        }

        public static INode CreateTypeAccessNode(IDataType nodeDataType)
        {
            var nodeTypePath = nodeDataType.FullName.Split('.');
            INode source = new IdNode(nodeTypePath[0]);
            for (var i = 1; i < nodeTypePath.Length; i++)
            {
                source = new PropertyAccessNode(source, new IdNode(nodeTypePath[i]));
            }

            return source;
        }
    }
}