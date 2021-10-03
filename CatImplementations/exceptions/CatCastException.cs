using CatApi.types;

namespace CatImplementations.exceptions
{
    public class CatCastException : CatException
    {
        public CatCastException(IDataType source, IDataType target) : base(
            $"Can't cast type {source.FullName} to type {target.FullName}")
        {
        }
    }
}