using System;
using CatLexing.tokens;

namespace CatAst.nodes.modules
{
    public class ClassDefinitionNode : IModuleObjectNode
    {
        public string Name { get; }
        
        public ClassDefinitionNode(IdNode name)
        {
            Name = name.IdToken;
        }

        public bool Exported { get; set; }
        public void SetExported(bool value)
        {
            Exported = value;
        }
    }
}