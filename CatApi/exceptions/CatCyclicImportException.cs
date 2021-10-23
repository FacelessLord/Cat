namespace CatApi.exceptions
{
    public class CatCyclicImportException : CatException
    {
        public CatCyclicImportException(string moduleName) : base(
            $"Found cyclic import while resolving module {moduleName}")
        {
        }
    }
}