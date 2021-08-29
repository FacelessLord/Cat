﻿using System;
using System.Collections.Generic;
using Cat.ast.api;
using Cat.lexing.tokens;

namespace Cat.ast
{
    public class Parser
    {
        public INode TryParse(IRule MainRule, Action<Token, string> onError, IEnumerable<Token> tokens)
        {
            var tokenBuffer = new BufferedEnumerable<Token>(tokens);
            if (MainRule.TryRead(tokenBuffer, onError, out var result))
            {
                return result;
            }

            return null;
        }

        public static void OnError(Token token, string message)
        {
            Console.Error.WriteLine($"Token <{token.Type},{token.Value}> {message}");
        }
    }
}