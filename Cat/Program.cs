﻿using System;
using System.IO;
using System.Linq;
using Cat.ast;
using Cat.ast.rules;
using Cat.lexing;

#nullable enable
namespace Cat
{
    class Program
    {
        static void Main(string[] args)
        {
            var lexer = new Lexer();
            var parser = new Parser();

            var code = string.Join("", File.ReadLines("./code.ct"));

            var tokens = lexer.ParseCode(code).ToList();
            var node = parser.TryParse(Contexts.FunctionBody, Parser.OnError, tokens);
            var a = 0;
        }
    }
}