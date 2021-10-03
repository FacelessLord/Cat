namespace CatApi.bytecode
{
    public interface IBytecodeContainer
    {
        public int WriteBytecodeBlock(IBytecodeBlock block);

        public IBytecodeBlock BuildBlock();
    }
}