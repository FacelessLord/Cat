using System.Collections.Generic;

namespace CatApi.bytecode
{
    public class BytecodeContainer : IBytecodeContainer
    {
        private readonly IBytecodeIndex _index;
        public List<IInstruction> Instructions = new();
        public BytecodeContainer(IBytecodeIndex index)
        {
            _index = index;
        }
        
        public int WriteBytecodeBlock(IBytecodeBlock block)
        {
            throw new System.NotImplementedException();
        }

        public IBytecodeBlock BuildBlock()
        {
            throw new System.NotImplementedException();
        }
    }
}