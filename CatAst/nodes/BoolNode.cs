﻿using System;
using Cat.lexing.tokens;

namespace Cat.ast.nodes
{
    public class BoolNode : INode
    {
        public string BoolToken { get; }

        public BoolNode(TokenNode numberToken)
        {
            if (numberToken.Token.Type != TokenTypes.Bool)
                throw new ArgumentException("Token have to have type Bool, not " + numberToken.Token.Type);
            BoolToken = numberToken.Token.Value;
        }
    }
}