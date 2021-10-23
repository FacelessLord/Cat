using CatApi.types;
using CatApi.types.dataTypes;

namespace CatApi.exceptions
{
    public class CatCastException : CatException
    {
        public CatCastException(IDataType source, IDataType target) : base(
            $"Can't cast type {source.FullName} to type {target.FullName}")
        {
        }
    }
}