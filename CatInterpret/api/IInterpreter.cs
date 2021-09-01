﻿using Cat.ast;

namespace Cat.interpret
{
    public interface IInterpreter<T>
    {
        public T Interpret(INode node);
    }
}