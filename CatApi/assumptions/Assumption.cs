using System;
using CatApi.types;
using CatApi.types.containers;
using CatApi.types.dataTypes;

namespace CatApi.assumptions
{
    public interface IAssumption : IDisposable
    {
        public void GotToBeWrong();
    }

    public static class Assumption
    {
        public static TypeAssumptionSourceTargetTypeConfig That(IDataType sourceTargetDataType)
        {
            return new(sourceTargetDataType);
        }
    }

    public class TypeAssumptionSourceTargetTypeConfig
    {
        private readonly IDataType _sourceTargetDataType;

        public TypeAssumptionSourceTargetTypeConfig(IDataType sourceTargetDataType)
        {
            _sourceTargetDataType = sourceTargetDataType;
        }

        public TypeAssumptionStorageConfig IsAssignableFrom(IDataType sourceDataType)
        {
            return new(sourceDataType, _sourceTargetDataType);
        }

        public TypeAssumptionStorageConfig IsAssignableTo(IDataType targetDataType)
        {
            return new(_sourceTargetDataType, targetDataType);
        }
    }

    public class TypeAssumptionStorageConfig
    {
        private readonly IDataType _sourceDataType;
        private readonly IDataType _targetDataType;

        public TypeAssumptionStorageConfig(IDataType sourceDataType, IDataType targetDataType)
        {
            _sourceDataType = sourceDataType;
            _targetDataType = targetDataType;
        }

        public TypeAssumption On(TypeStorage typeStorage)
        {
            return new(_sourceDataType, _targetDataType, typeStorage);
        }
    }

    public class TypeAssumption : IAssumption
    {
        private readonly IDataType _sourceDataType;
        private readonly IDataType _targetDataType;
        private readonly TypeStorage _typeStorage;
        private bool _isKnownBeforeAssumption;

        public TypeAssumption(IDataType sourceDataType, IDataType targetDataType, TypeStorage typeStorage)
        {
            _sourceDataType = sourceDataType;
            _targetDataType = targetDataType;
            _typeStorage = typeStorage;

            var cacheKey = (_sourceDataType.FullName, _targetDataType.FullName);
            _isKnownBeforeAssumption = _typeStorage.AssignabilityCache.ContainsKey(cacheKey);
            if (!_isKnownBeforeAssumption)
                typeStorage.AssignabilityCache[cacheKey] = true;
        }

        public void Dispose()
        {
            var cacheKey = (_sourceDataType.FullName, _targetDataType.FullName);
            if (!_isKnownBeforeAssumption)
            {
                _typeStorage.AssignabilityCache.Remove(cacheKey);
            }
        }

        public void GotToBeWrong()
        {
            var cacheKey = (_sourceDataType.FullName, _targetDataType.FullName);
            _typeStorage.AssignabilityCache[cacheKey] = false;

            Dispose();
        }

        public void GotToBeTrue()
        {
            _isKnownBeforeAssumption = true;
        }
    }
}