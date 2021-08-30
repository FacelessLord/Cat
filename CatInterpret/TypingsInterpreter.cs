using System;
using Cat.ast;
using Cat.ast.nodes;
using Cat.data.types;
using Cat.data.types.api;
using Cat.data.types.primitives;

namespace Cat.interpret
{
    public class TypingsInterpreter : IInterpreter<IDataType>
    {
        private readonly ITypeStorage _typeStorage;

        public TypingsInterpreter(ITypeStorage typeStorage)
        {
            _typeStorage = typeStorage;
        }
        
        public IDataType Interpret(INode node)
        {
            return node switch
            {
                NumberNode n => Interpret(n),
                StringNode n => Interpret(n),
                _ => throw new InvalidOperationException()
            };
        }
        
        public IDataType Interpret(NumberNode node)
        {
            return _typeStorage[Primitives.Number];
        }
        
        public IDataType Interpret(StringNode node)
        {
            return _typeStorage[Primitives.String];
        }
    }
}