using CatApi.modules;

namespace CatApi.exceptions
{
    public class CatModuleRedeclarationException : CatException
    {
        public CatModuleRedeclarationException(IUnboundModule a, IUnboundModule b) : base(
            $"Module {a.Name} at {a.Path} has already been defined at {b.Path} ")
        {
        }
    }
}