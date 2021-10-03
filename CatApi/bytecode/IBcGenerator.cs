namespace CatApi.bytecode
{
    public interface IBytecodeGenerator
    {
        public IBytecodeBlock GenerateCodeFor(IBytecodeCompilable compilable);
    }
}