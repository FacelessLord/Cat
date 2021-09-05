﻿using CatLexing.tokens;

namespace CatAst.api
{
    public interface IRule
    {
        public INode Read(BufferedEnumerable<Token> tokens);
    }
}