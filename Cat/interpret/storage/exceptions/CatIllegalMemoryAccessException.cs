namespace Cat.interpret.storage.exceptions
{
    public class CatIllegalMemoryAccessException : CatMemoryException
    {
        public CatIllegalMemoryAccessException(IRef @ref) : base(
            $"Couldn't read object from memory at ref<{@ref.AsMemoryAddress()}>")
        {
        }
    }
}