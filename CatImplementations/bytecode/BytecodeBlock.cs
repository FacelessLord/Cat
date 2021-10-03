using System.Collections.Generic;
using System.Linq;
using CatApi.bytecode;

namespace CatImplementations.bytecode
{
    public class BytecodeBlock : IBytecodeBlock
    {
        public IEnumerable<IInstruction> Instructions
        {
            get => _instructionList;
        }

        private List<IInstruction> _instructionList;

        public BytecodeBlock(IEnumerable<IInstruction> instructions)
        {
            _instructionList = instructions.ToList();
        }
    }
}