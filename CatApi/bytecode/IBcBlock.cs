using System.Collections.Generic;

namespace CatApi.bytecode
{
    public interface IBytecodeBlock
    {
        public IEnumerable<IInstruction> Instructions { get; }
    }
}