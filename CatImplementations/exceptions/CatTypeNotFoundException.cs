namespace CatImplementations.exceptions
{
    public class CatTypeNotFoundException : CatTypeException
    {
        public CatTypeNotFoundException(string typeFullName) :
            base($"Type {typeFullName} cannot be found in assembly")
        {
        }
    }
}