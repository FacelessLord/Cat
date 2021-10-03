using System.Linq;
using CatApi.interpreting;
using CatApi.types;
using CatImplementations.ast.nodes;
using CatImplementations.exceptions;

namespace CatImplementations.typings
{
    public static class CastFunctionsHelper
    {
        public const string CastFunctionBaseName = "object.castTo";

        public static string CreateCastFunctionName(IDataType type) => CastFunctionBaseName + type.FullName;

        public static bool TryGetCastMethodName(ITypeStorage typeStorage, IDataType source, IDataType targetType,
            out string castMethodName)
        {
            var suitableTargetTypes = source.Properties.Select(p => p.Name)
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

        public static INode AddTypeCastIfNeeded(ITypeStorage typeStorage, INode node, IDataType nodeType,
            IDataType targetType)
        {
            if (typeStorage.IsTypeAssignableFrom(targetType, nodeType))
            {
                return node;
            }

            if (TryGetCastMethodName(typeStorage, nodeType, targetType, out var castMethodName))
            {
                var source = CreateTypeAccessNode(nodeType);
                source = CreatePropertyAccessNode(source, castMethodName);
                source = new ObjectCallNode(source, new ExpressionListNode(node));

                return source;
            }

            throw new CatCastException(nodeType, targetType);
        }

        public static INode CreatePropertyAccessNode(INode source, string propertyName)
        {
            return new PropertyAccessNode(source, new IdNode(propertyName));
        }

        public static INode ObjectCallNode(INode source, INode[] args)
        {
            return new ObjectCallNode(source, new ExpressionListNode(args));
        }

        public static INode CreateTypeAccessNode(IDataType nodeType)
        {
            var nodeTypePath = nodeType.FullName.Split('.');
            INode source = new IdNode(nodeTypePath[0]);
            for (var i = 1; i < nodeTypePath.Length; i++)
            {
                source = new PropertyAccessNode(source, new IdNode(nodeTypePath[i]));
            }

            return source;
        }
    }
}