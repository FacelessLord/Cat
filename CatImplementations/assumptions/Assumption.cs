

using CatApi.assumptions;
using CatApi.types;
using CatImplementations.typings;

namespace CatImplementations.assumptions
{
    public static class Assumption
    {
        public static TypeAssumptionSourceTargetTypeConfig That(IDataType sourceTargetType)
        {
            return new TypeAssumptionSourceTargetTypeConfig(sourceTargetType);
        }
    }

    public class TypeAssumptionSourceTargetTypeConfig
    {
        private readonly IDataType _sourceTargetType;

        public TypeAssumptionSourceTargetTypeConfig(IDataType sourceTargetType)
        {
            _sourceTargetType = sourceTargetType;
        }

        public TypeAssumptionStorageConfig IsAssignableFrom(IDataType sourceType)
        {
            return new TypeAssumptionStorageConfig(sourceType, _sourceTargetType);
        }

        public TypeAssumptionStorageConfig IsAssignableTo(IDataType targetType)
        {
            return new TypeAssumptionStorageConfig(_sourceTargetType, targetType);
        }
    }

    public class TypeAssumptionStorageConfig
    {
        private readonly IDataType _sourceType;
        private readonly IDataType _targetType;

        public TypeAssumptionStorageConfig(IDataType sourceType, IDataType targetType)
        {
            _sourceType = sourceType;
            _targetType = targetType;
        }

        public TypeAssumption On(TypeStorage typeStorage)
        {
            return new TypeAssumption(_sourceType, _targetType, typeStorage);
        }
    }

    public class TypeAssumption : IAssumption
    {
        private readonly IDataType _sourceType;
        private readonly IDataType _targetType;
        private readonly TypeStorage _typeStorage;
        private bool _isKnownBeforeAssumption;

        public TypeAssumption(IDataType sourceType, IDataType targetType, TypeStorage typeStorage)
        {
            _sourceType = sourceType;
            _targetType = targetType;
            _typeStorage = typeStorage;

            var cacheKey = (_sourceType.FullName, _targetType.FullName);
            _isKnownBeforeAssumption = _typeStorage.AssignabilityCache.ContainsKey(cacheKey);
            if (!_isKnownBeforeAssumption)
                typeStorage.AssignabilityCache[cacheKey] = true;
        }

        public void Dispose()
        {
            var cacheKey = (_sourceType.FullName, _targetType.FullName);
            if (!_isKnownBeforeAssumption)
            {
                _typeStorage.AssignabilityCache.Remove(cacheKey);
            }
        }

        public void GotToBeWrong()
        {
            var cacheKey = (_sourceType.FullName, _targetType.FullName);
            _typeStorage.AssignabilityCache[cacheKey] = false;
            
            Dispose();
        }

        public void GotToBeTrue()
        {
            _isKnownBeforeAssumption = true;
        }
    }
}