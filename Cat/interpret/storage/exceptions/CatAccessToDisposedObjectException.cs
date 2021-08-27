namespace Cat.interpret.storage.exceptions
{
    public class CatAccessToDisposedObjectException : CatMemoryException
    {
        public CatAccessToDisposedObjectException(IRef @ref) : base(
            $"Object at ref<{@ref.AsMemoryAddress()}> had already been disposed")
        {
        }
    }
}